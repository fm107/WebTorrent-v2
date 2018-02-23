using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using WebTorrent.Data.Models.Interfaces;

namespace WebTorrent.Data.Models
{
    public class Content : IEntity 
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string TorrentName { get; set; }
        public string Hash { get; set; }
        public bool IsInProgress { get; set; }
        public string RelativePath { get; set; }

        [JsonIgnore]
        public int ParentId { get; set; }
        [JsonIgnore]
        public int Lft { get; set; }
        [JsonIgnore]
        public int Rgt { get; set; }
        public virtual List<FileSystemItem> FsItems { get; set; }
    }

    public class TreeMap : IEntity
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int Lft { get; set; }
        public int Rgt { get; set; }
    }

    public class FileSystemItem : IEntity
    {
        [JsonIgnore]
        public int Id { get; set; }

        public bool IsStreaming { get; set; }
        public string Stream { get; set; }
        public string DownloadPath { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime LastChanged { get; set; }
        public FsType Type { get; set; }
    }

    public enum FsType
    {
        Folder,
        File
    }
}