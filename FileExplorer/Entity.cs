using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileExplorer
{
    struct Attribute
    {
        public bool IsWriteProtected;
        public bool IsHidden;
        public bool IsSystemFile;
        public bool IsVolumeName;
        public bool IsDirectory;
    }
    class Entity
    {
        protected byte[] buffer;
        protected Directory parent;
        protected FAT fat;
        private string longName;
        private int clusterIndex;

        public string LongName
        {
            get
            {
                return longName ?? Name;
            }
            set
            {
                this.longName = value;
            }
        }

        public Entity(byte[] Buffer, FAT fat, int clusterIndex, Directory parent)
        {
            this.buffer = Buffer;
            this.parent = parent;
            this.fat = fat;
            this.clusterIndex = clusterIndex;
        }

        public string Name
        {
            get
            {
                return Encoding.ASCII.GetString(buffer.ToList().GetRange(0, 8).ToArray()).Trim() + (Attribute.IsDirectory ? "" : "." + Encoding.ASCII.GetString(buffer.ToList().GetRange(8, 3).ToArray()).ToLower());
            }
        }

        public Attribute Attribute
        {
            get
            {
                Attribute temp;
                BitArray bits = new BitArray(buffer.ToList().GetRange(11, 1).ToArray());
                temp.IsHidden = bits.Get(1);
                temp.IsDirectory = bits.Get(4);
                temp.IsSystemFile = bits.Get(2);
                temp.IsVolumeName = bits.Get(3);
                temp.IsWriteProtected = bits.Get(0);
                return temp;
            }
        }

        public byte[] Reserved
        {
            get
            {
                return buffer.ToList().GetRange(12, 10).ToArray();
            }
        }

        public DateTime LastChange
        {
            get
            {
                var time = getTime(buffer.ToList().GetRange(22, 2).ToArray());
                var date = getDate(buffer.ToList().GetRange(24, 2).ToArray());
                var dateTime = new DateTime(date[0], date[1], date[2], time[0], time[1], time[2]);
                return dateTime;
            }
        }

        public int FirstClusterOfFile
        {
            get
            {
                return buffer.ToList().GetRange(26, 2).ToArray().ToInt32();
            }
        }
        public int ClusterIndex
        {
            get
            {
                return clusterIndex;
            }
        }

        virtual public void Delete()
        {
            if (IsDeleted)
                throw new Exception();
            throw new NotImplementedException();
        }

        virtual public void UnDelete()
        {
            if (!IsDeleted)
                throw new Exception();
            throw new NotImplementedException();
        }

        public void ToggleDeleteUndelete()
        {
            if (this.IsDeleted)
                this.UnDelete();
            else
                this.Delete();
        }

        public bool IsDeleted
        {
            get
            {
                return buffer[0] == 0xe5;
            }
        }

        public override string ToString()
        {
            return LongName ?? Name;
        }
        public static int[] getDate(byte[] date_)
        {
            int[] result = new int[3];
            int year = 0, month = 0, day = 0;
            day = date_[0];
            if (day >= 128)
            {
                day -= 128;
                month += 4;
            }
            if (day >= 64)
            {
                day -= 64;
                month += 2;
            }
            if (day >= 32)
            {
                day -= 32;
                month += 1;
            }
            if ((date_[1] & (1 << 0)) != 0)
            {
                month += 8;
            }
            if ((date_[1] & (1 << 1)) != 0)
            {
                year += 1;
            }
            if ((date_[1] & (1 << 2)) != 0)
            {
                year += 2;
            }
            if ((date_[1] & (1 << 3)) != 0)
            {
                year += 4;
            }
            if ((date_[1] & (1 << 4)) != 0)
            {
                year += 8;
            }
            if ((date_[1] & (1 << 5)) != 0)
            {
                year += 16;
            }
            if ((date_[1] & (1 << 6)) != 0)
            {
                year += 32;
            }
            if ((date_[1] & (1 << 7)) != 0)
            {
                year += 64;
            }
            year += 1980;
            result[0] = year;
            result[1] = month;
            result[2] = day;
            return result;
        }
        public static int[] getTime(byte[] time_)
        {
            int[] result = new int[3];
            int sec = 0, min = 0, hour = 0;
            sec = time_[0];
            if (sec >= 128)
            {
                sec -= 128;
                min += 4;
            }
            if (sec >= 64)
            {
                sec -= 64;
                min += 2;
            }
            if (sec >= 32)
            {
                sec -= 32;
                min += 1;
            }
            sec *= 2;
            if ((time_[1] & (1 << 0)) != 0)
            {
                min += 8;
            }
            if ((time_[1] & (1 << 1)) != 0)
            {
                min += 16;
            }
            if ((time_[1] & (1 << 2)) != 0)
            {
                min += 32;
            }
            if ((time_[1] & (1 << 3)) != 0)
            {
                hour += 1;
            }
            if ((time_[1] & (1 << 4)) != 0)
            {
                hour += 2;
            }
            if ((time_[1] & (1 << 5)) != 0)
            {
                hour += 4;
            }
            if ((time_[1] & (1 << 6)) != 0)
            {
                hour += 8;
            }
            if ((time_[1] & (1 << 7)) != 0)
            {
                hour += 16;
            }
            result[0] = hour;
            result[1] = min;
            result[2] = sec;
            return result;
        }
    }

    class Directory : Entity
    {
        private List<File> subFiles;
        private List<Directory> subDirectories;
        public System.Windows.Forms.TreeNode ToTreeNode()
        {
            var tn = new System.Windows.Forms.TreeNode(this.LongName) { Tag = this };

            if (this.Attribute.IsHidden)
                tn.ForeColor = System.Drawing.Color.SlateGray;

            if (this.IsDeleted)
                tn.ForeColor = System.Drawing.Color.Red;

            return tn;
        }

        public Directory(byte[] Buffer, FAT fat, int ParentClusterNumber, Directory parent)
            : base(Buffer, fat, ParentClusterNumber, parent)
        {
            subFiles = new List<File>();
            subDirectories = new List<Directory>();
        }

        private void getSubEntities()
        {
            uint length;
            List<int> clusters = fat.getClusterTail(FirstClusterOfFile);
            int numberOfBytesPerCluster = fat.SectorsPerCluster * fat.BytesPerSector;
            byte[] buffer = new byte[clusters.Count() * numberOfBytesPerCluster];
            subDirectories.Clear();
            subFiles.Clear();
            for (int i = 0; i < clusters.Count(); i++)
            {
                byte[] temp = IO.read(fat.HandleFile, clusters[i], fat.ClusterSize, fat);
                for (int j = 0; j < temp.Length; j++)
                {
                    buffer[i * numberOfBytesPerCluster + j] = temp[j];
                }
            }

            Stack<byte[]> longName = new Stack<byte[]>();
            for (int i = 0; i < buffer.Length; i += 32)
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
                    Entity temp = new Entity(buffer.ToList().GetRange(i, 32).ToArray(), fat, clusters[(i + 1) / numberOfBytesPerCluster], this);
                    string tempName = null;
                    for (; longName.Count != 0; )
                    {
                        var x = longName.Pop();
                        tempName += Encoding.Unicode.GetString(x.ToList().GetRange(1, 10).ToArray()) +
                                    Encoding.Unicode.GetString(x.ToList().GetRange(14, 12).ToArray()) +
                                    Encoding.Unicode.GetString(x.ToList().GetRange(28, 4).ToArray());
                    }
                    temp.LongName = tempName != null ? tempName.Split('\0')[0] : null;
                    if (temp.Attribute.IsDirectory)
                    {
                        Directory di = new Directory(buffer.ToList().GetRange(i, 32).ToArray(), fat, clusters[(i + 1) / numberOfBytesPerCluster], this);
                        di.LongName = tempName != null ? tempName.Split('\0')[0] : null;
                        subDirectories.Add(di);
                    }
                    else
                    {
                        File fi = new File(buffer.ToList().GetRange(i, 32).ToArray(), fat, clusters[(i + 1) / numberOfBytesPerCluster], this);
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
        new public void Delete()
        {
            for (int i = 0; i < this.subFiles.Count; i++)
            {
                if (!this.subFiles[i].IsDeleted)
                    this.subFiles[i].Delete();
            }
            for (int i = 0; i < this.subDirectories.Count; i++)
            {
                if (this.subDirectories[i].Name == "." || this.subDirectories[i].Name == "..")
                    continue;
                this.subDirectories[i].Delete();
            }
            IO.edit(fat.HandleFile, this.ClusterIndex, this.buffer, fat);
        }
        new public void UnDelete()
        {
            IO.edit(fat.HandleFile, this.ClusterIndex, this.buffer, 0x61, fat);
            for (int i = 0; i < this.subFiles.Count; i++)
            {
                if (this.subFiles[i].IsDeleted)
                    this.subFiles[i].UnDelete();
            }
            for (int i = 0; i < this.subDirectories.Count; i++)
            {
                if (this.subDirectories[i].Name == "." || this.subDirectories[i].Name == "..")
                    continue;
                this.subDirectories[i].UnDelete();
            }
        }
        new public void ToggleDeleteUndelete()
        {
            if (this.IsDeleted)
                this.UnDelete();
            else
                this.Delete();
        }
    }

    class File : Entity
    {
        public File(byte[] buffer, FAT fat, int ParentClusterNumber, Directory parent)
            : base(buffer, fat, ParentClusterNumber, parent)
        {
        }

        public int Size
        {
            get
            {
                return buffer.ToList().GetRange(28, 4).ToArray().ToInt32();
            }
        }

        public string Extention
        {
            get
            {
                try
                {
                    return LongName.Split('.')[Name.Split('.').Length - 1].ToLower();
                }
                catch
                {
                    return "";
                }
            }
        }

        public System.Windows.Forms.ListViewItem ToListItem()
        {
            var s = new string[]
            {
                this.LongName,
                this.Extention,
                (this.Size / 1024).ToString() + " kb",
                this.IsDeleted.YesNo(),
                this.Attribute.IsHidden.YesNo(),
                this.Attribute.IsSystemFile.YesNo(),
                this.Attribute.IsWriteProtected.YesNo(),
                this.LastChange.ToString()
            };
            var li = new System.Windows.Forms.ListViewItem(s) { Tag = this };
            if (this.IsDeleted)
                li.ForeColor = System.Drawing.Color.Red;
            if (this.Attribute.IsHidden)
                li.ForeColor = System.Drawing.Color.SlateGray;

            return li;
        }
        public void Delete()
        {
            IO.edit(fat.HandleFile, this.ClusterIndex, this.buffer, fat);
        }
        public void UnDelete()
        {
            IO.edit(fat.HandleFile, this.ClusterIndex, this.buffer, 0x61, fat);
        }
        public void ToggleDeleteUndelete()
        {
            if (this.IsDeleted)
                this.UnDelete();
            else
                this.Delete();
        }
    }
}