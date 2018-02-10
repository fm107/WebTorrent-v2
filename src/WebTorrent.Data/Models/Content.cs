using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using WebTorrent.Data.Models.Interfaces;

namespace WebTorrent.Data.Models
{
    public class Content : IEntity
    {
        public string TorrentName { get; set; }
        public string Hash { get; set; }
        public bool IsInProgress { get; set; }
        public string CurrentFolder { get; set; }
        public string ParentFolder { get; set; }
        public virtual List<FileSystemItem> FsItems { get; set; }

        [JsonIgnore] public int Id { get; set; }
    }

    public class FileSystemItem : IEntity
    {
        public bool IsStreaming { get; set; }
        public string Stream { get; set; }
        public string DownloadPath { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime LastChanged { get; set; }
        public string Type { get; set; }
        public int Id { get; set; }
    }
}