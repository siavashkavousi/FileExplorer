using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace FileExplorer
{
    class IO
    {
        private const int ENTITY_SIZE = 32;
        private const uint GENERIC_READ = (0x80000000);
        private const uint GENERIC_WRITE = (0x40000000);
        private const uint GENERIC_EXECUTE = (0x20000000);
        private const uint GENERIC_ALL = (0x10000000);

        #region dll imports
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "CreateFileW", SetLastError = true)]
        extern static IntPtr CreateFile(String lpFileName,
                                 uint dwDesiredAccess,
                                 uint dwShareMode,
                                 IntPtr lpSecurityAttributes,
                                 uint dwCreationDisposition,
                                 uint dwFlagsAndAttributes,
                                 IntPtr hTemplateFile);

        [DllImport("kernel32.dll", EntryPoint = "GetLastError", SetLastError = true)]
        extern static int GetLastError();

        [DllImport("kernel32.dll", EntryPoint = "CloseHandle", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        extern static bool CloseHandle(IntPtr hHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadFile(IntPtr hFile, [Out] byte[] lpBuffer,
           uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadFile(IntPtr hFile, IntPtr lpBuffer, uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", EntryPoint = "SetFilePointerEx")]
        extern static bool SetFilePointerEx(IntPtr hFile, long liDistanceToMove, out IntPtr lpNewFilePointer, uint dwMoveMethod);
        [DllImport("kernel32.dll", EntryPoint = "WriteFile", SetLastError = true)]
        extern static bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, [In] IntPtr lpOverlapped);
        #endregion

        #region IO methods
        public static IntPtr createFile(string driverLetter)
        {
            return CreateFile(string.Concat("\\\\.\\", driverLetter), GENERIC_READ | GENERIC_WRITE, 0x00000001, IntPtr.Zero, 3, 0, IntPtr.Zero);
        }
        public static byte[] read(IntPtr hFile, int index, FAT fat)
        {
            return read(hFile, index, 512, fat);
        }
        public static byte[] read(IntPtr hFile, int index, int size, FAT fat)
        {
            seek(hFile, index, fat);
            if (size % 512 != 0)
                throw new Exception("Size should be a coefficient of 512");
            uint length = 0;
            var buffer = new byte[size];
            if (!ReadFile(hFile, buffer, (uint)buffer.Length, out length, IntPtr.Zero))
            {
                throw new Exception("Error reading file");
            }
            return buffer;
        }
        public static bool write(IntPtr hFile, int index, byte[] buffer, FAT fat)
        {
            seek(hFile, index, fat);
            uint length = 0;
            if (!WriteFile(hFile, buffer, (uint)buffer.Length, out length, IntPtr.Zero))
            {
                int error = GetLastError();
                throw new Exception("Error writing file" + error);
            }
            return true;
        }

        public static void edit(IntPtr hFile, int clusterIndex, byte[] entity, FAT fat)
        {
            edit(hFile, clusterIndex, entity, 0xE5, fat);
        }
        public static void edit(IntPtr hFile, int clusterIndex, byte[] entity, byte head, FAT fat)
        {
            byte[] temp = new byte[ENTITY_SIZE];
            Array.Copy(entity, temp, 32);
            temp[0] = head;
            edit(hFile, clusterIndex, entity, temp, fat);
        }
        public static void edit(IntPtr hFile, int clusterIndex, byte[] entity, byte[] newEntity, FAT fat)
        {
            if (clusterIndex == (int)LOCATION.ROOT_DIRECTORY)
            {
                int SIZE = fat.RootDirectorySize;
                byte[] buffer = read(hFile, (int)LOCATION.ROOT_DIRECTORY, SIZE, fat);
                for (int i = 0; i < SIZE / ENTITY_SIZE; i++)
                {
                    int j = i * ENTITY_SIZE;
                    byte[] temp = new byte[ENTITY_SIZE];
                    Array.Copy(buffer, j, temp, 0, 32);
                    if (temp[0] == 0) break;
                    if (temp.SequenceEqual(entity))
                    {
                        Array.Copy(newEntity, 0, buffer, j, 32);
                        write(hFile, (int)LOCATION.ROOT_DIRECTORY, buffer, fat);
                        int x = entity.ToList().GetRange(26, 2).ToArray().ToInt32();
                        if (entity[0].Equals(0xE5) && !newEntity.Equals(0xE5))
                            undeleteClusterTail(hFile, x, fat);
                        else if (newEntity[0].Equals(0xE5))
                            deleteClusterTail(hFile, x, fat);
                        break;
                    }
                }
            }
            else
            {
                int SIZE = fat.ClusterSize;
                byte[] buffer = read(hFile, clusterIndex, SIZE, fat);
                for (int i = 0; i < SIZE / ENTITY_SIZE; i++)
                {
                    int j = i * ENTITY_SIZE;
                    byte[] temp = new byte[ENTITY_SIZE];
                    Array.Copy(buffer, j, temp, 0, 32);
                    if (temp[0] == 0) break;
                    if (temp.SequenceEqual(entity))
                    {
                        Array.Copy(newEntity, 0, buffer, j, 32);
                        write(hFile, clusterIndex, buffer, fat);
                        int x = entity.ToList().GetRange(26, 2).ToArray().ToInt32();
                        if (entity[0].Equals(0xE5) && !newEntity.Equals(0xE5))
                            undeleteClusterTail(hFile, x, fat);
                        else if (newEntity[0].Equals(0xE5))
                            deleteClusterTail(hFile, x, fat);
                        break;
                    }
                }
            }
        }
        public static void quickFormat(IntPtr hFile, FAT fat)
        {
            byte[] buffer = read(hFile, (int)LOCATION.FAT_TABLE_1, fat);
            for (int i = 8; i < buffer.Length; i++)
            {
                if (buffer[i] == 0) continue;
                else buffer[i] = 0;
            }
            write(hFile, (int)LOCATION.FAT_TABLE_1, buffer, fat);
            buffer = read(hFile, (int)LOCATION.FAT_TABLE_2, fat);
            for (int i = 8; i < buffer.Length; i++)
            {
                if (buffer[i] == 0) continue;
                else buffer[i] = 0;
            }
            write(hFile, (int)LOCATION.FAT_TABLE_2, buffer, fat);
            buffer = read(hFile, (int)LOCATION.ROOT_DIRECTORY, fat);
            for (int i = 96; i < buffer.Length; i++)
            {
                if (buffer[i] == 0) continue;
                else buffer[i] = 0;
            }
            write(hFile, (int)LOCATION.ROOT_DIRECTORY, buffer, fat);
            int SIZE = fat.SectorsInVolume * fat.BytesPerSector - fat.RootDirectorySize;
            buffer = read(hFile, 0, SIZE/2,fat);
            for (int i = 0; i < buffer.Length/2; i++)
            {
                if (buffer[i] == 0) continue;
                else buffer[i] = 0;
            }
            write(hFile, 0, buffer, fat);
        }
        public static bool seek(IntPtr hFile, int index, FAT fat)
        {
            if (index == (int)LOCATION.BOOT_SECTOR)
            {
                return true;
            }
            else if (index == (int)LOCATION.FAT_TABLE_1)
            {
                seek(hFile, fat.FATTableBlock_1, 0, fat.BytesPerSector);
                return true;
            }
            else if (index == (int)LOCATION.FAT_TABLE_2)
            {
                seek(hFile, fat.FATTableBlock_2, 0, fat.BytesPerSector);
                return true;
            }
            else if (index == (int)LOCATION.ROOT_DIRECTORY)
            {
                seek(hFile, fat.RootDirectoryBlock, 0, fat.BytesPerSector);
                return true;
            }
            else if (index > 0)
            {
                seek(hFile, fat.DataAreaBlock + (index - 2) * fat.SectorsPerCluster, 0, fat.BytesPerSector);
                return true;
            }
            return false;
        }
        private static IntPtr seek(IntPtr hFile, int index, uint moveMethod, int bytesPerSector)
        {
            IntPtr newAddress;
            if (!SetFilePointerEx(hFile, index * bytesPerSector, out newAddress, moveMethod))
            {
                throw new Exception("Error seeking file");
            }
            return newAddress;
        }
        public static void closeHandle(IntPtr hFile)
        {
            if (!CloseHandle(hFile))
            {
                throw new Exception("Error Closing file");
            }
        }
        #endregion

        public static void deleteClusterTail(IntPtr hFile, int clusterNumber, FAT fat)
        {
            byte[] buffer = read(hFile, (int)LOCATION.FAT_TABLE_1, fat.FATTableSize, fat);
            int nextCluster = clusterNumber;
            do
            {
                int temp = buffer.ToList().GetRange(nextCluster * 2, 2).ToArray().ToInt32();
                buffer[nextCluster * 2] = 0;
                buffer[nextCluster * 2 + 1] = 0;
                nextCluster = temp;
            } while (!(nextCluster == 0xFFF8 || nextCluster == 0xFFFF));
            write(hFile, (int)LOCATION.FAT_TABLE_1, buffer, fat);
        }
        public static void undeleteClusterTail(IntPtr hFile, int clusterNumber, FAT fat)
        {
            byte[] buffer_1 = read(hFile, (int)LOCATION.FAT_TABLE_1, fat.FATTableSize, fat);
            byte[] buffer_2 = read(hFile, (int)LOCATION.FAT_TABLE_2, fat.FATTableSize, fat);
            int nextCluster = clusterNumber;
            do
            {
                int temp = buffer_2.ToList().GetRange(nextCluster * 2, 2).ToArray().ToInt32();
                buffer_1[nextCluster * 2] = buffer_2[nextCluster * 2];
                buffer_1[nextCluster * 2 + 1] = buffer_2[nextCluster * 2 + 1];
                nextCluster = temp;
            } while (!(nextCluster == 0xFFF8 || nextCluster == 0xFFFF));
            write(hFile, (int)LOCATION.FAT_TABLE_1, buffer_1, fat);
        }
    }
}
