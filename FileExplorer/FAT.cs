using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer
{
    enum LOCATION { BOOT_SECTOR = -4, FAT_TABLE_1 = -3, FAT_TABLE_2 = -2, ROOT_DIRECTORY = -1 };
    class FAT
    {
        private IntPtr hFile;
        private List<Directory> subDirectories;
        private List<File> subFiles;
        private byte[] buffer;
        private const int ENTITY_SIZE = 32;

        #region bootsector properties
        public int BytesPerSector
        {
            get
            {
                return littleEndian(buffer, 11, 12);
            }
        }
        public int SectorsPerCluster
        {
            get
            {
                return buffer[13];
            }
        }
        public int ReservedSectors
        {
            get
            {
                return littleEndian(buffer, 14, 15);
            }
        }
        public int NumberOfFats
        {
            get
            {
                return buffer[16];
            }
        }
        public int NumberOfEntries
        {
            get
            {
                return littleEndian(buffer, 17, 18);
            }
        }
        public int SectorsInVolume
        {
            get
            {
                return littleEndian(buffer, 19, 20);
            }
        }
        public int MediaDescriptor
        {
            get
            {
                return buffer[21];
            }
        }
        public int SectorsPerFat
        {
            get
            {
                return littleEndian(buffer, 22, 23);
            }
        }
        public int RootDirectorySize
        {
            get
            {
                return NumberOfEntries * ENTITY_SIZE;
            }
        }
        public int ClusterSize
        {
            get
            {
                return SectorsPerCluster * BytesPerSector;
            }
        }
        public int FATTableSize
        {
            get
            {
                return SectorsPerFat * BytesPerSector;
            }
        }
        public int RootDirectoryBlock
        {
            get
            {
                return SectorsPerFat * NumberOfFats + ReservedSectors;
            }
        }
        public int FATTableBlock_1
        {
            get
            {
                return ReservedSectors;
            }
        }
        public int FATTableBlock_2
        {
            get
            {
                return ReservedSectors + FATTableSize / BytesPerSector;
            }
        }
        public int DataAreaBlock
        {
            get
            {
                return RootDirectoryBlock + RootDirectorySize / BytesPerSector;
            }
        }
        public int littleEndian(byte[] buffer, int LSB, int MSB)
        {
            int x = buffer[LSB];
            int y = buffer[MSB];
            return (y << 8) + x;
        }
        #endregion

        #region file explorer methods
        private void getSubEntities()
        {
            subDirectories.Clear();
            subFiles.Clear();
            int rootSize = RootDirectorySize;
            byte[] buffer = IO.read(hFile, (int)LOCATION.ROOT_DIRECTORY, rootSize, this);
            Stack<byte[]> longName = new Stack<byte[]>();
            for (int i = 0; i < rootSize; i += 32)
            {
                if (buffer[i] == 0)
                {
                    //no more entity beyond
                    break;
                }
                else if (buffer[i + 11] == 0x0f)
                {
                    longName.Push(buffer.ToList().GetRange(i, 32).ToArray());
                }
                else
                {
                    Entity temp = new Entity(buffer.ToList().GetRange(i, 32).ToArray(), this, (int)LOCATION.ROOT_DIRECTORY, null);
                    string tempName = null;
                    while (longName.Count != 0)
                    {
                        var x = longName.Pop();
                        tempName += Encoding.Unicode.GetString(x.ToList().GetRange(1, 10).ToArray()) +
                                Encoding.Unicode.GetString(x.ToList().GetRange(14, 12).ToArray()) +
                                Encoding.Unicode.GetString(x.ToList().GetRange(28, 4).ToArray());
                    }
                    if (temp.Attribute.IsDirectory)
                    {
                        Directory di = new Directory(buffer.ToList().GetRange(i, 32).ToArray(), this, (int)LOCATION.ROOT_DIRECTORY, null);
                        di.LongName = tempName != null ? tempName.Split('\0')[0] : null;
                        subDirectories.Add(di);
                    }
                    else
                    {
                        File fi = new File(buffer.ToList().GetRange(i, 32).ToArray(), this, (int)LOCATION.ROOT_DIRECTORY, null);
                        fi.LongName = tempName != null ? tempName.Split('\0')[0] : null;
                        subFiles.Add(fi);
                    }
                    longName.Clear();
                }
            }
        }
        public List<Directory> SubDirectories
        {
            get
            {
                getSubEntities();
                return subDirectories;
            }
        }

        public List<File> SubFiles
        {
            get
            {
                getSubEntities();
                return subFiles;
            }
        }
        public List<int> getClusterTail(int ClusterNumber)
        {
            byte[] buffer = IO.read(hFile, (int)LOCATION.FAT_TABLE_1, FATTableSize, this);
            List<int> result = new List<int>();
            int nextCluster = ClusterNumber;
            do
            {
                result.Add(nextCluster);
                nextCluster = buffer.ToList().GetRange(nextCluster * 2, 2).ToArray().ToInt32();
            } while (!(nextCluster == 0xFFF8 || nextCluster == 0xFFFF));
            return result;
        }
        public IntPtr HandleFile
        {
            get
            {
                return hFile;
            }
        }
        #endregion


        public FAT(string driveLetter)
        {
            hFile = IO.createFile(driveLetter);
            subFiles = new List<File>();
            subDirectories = new List<Directory>();

            buffer = IO.read(hFile, (int)LOCATION.BOOT_SECTOR, this);
            System.IO.File.WriteAllBytes("C:\\Users\\siavash\\Desktop\\sia.txt", buffer);
        }
    }
}
