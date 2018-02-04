using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTorrent.Model
{
    public class TorrentInfo
    {
        private double _progress;

        /// <summary>
        /// Integer in bytes
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// integer in per mils
        /// </summary>
        public double Progress
        {
            get => _progress / 10.0;
            set => _progress = value;
        }
        
        /// <summary>
        /// integer in bytes
        /// </summary>
        public long Downloaded { get; set; }

        /// <summary>
        /// integer in bytes
        /// </summary>
        public long Uploaded { get; set; }

        /// <summary>
        /// integer in per mils
        /// </summary>
        public int Ratio { get; set; }

        /// <summary>
        /// integer in bytes per second
        /// </summary>
        public int UploadSpeed { get; set; }

        /// <summary>
        /// integer in bytes per second
        /// </summary>
        public int DownloadSpeed { get; set; }

        /// <summary>
        /// integer in seconds
        /// </summary>
        public int Eta { get; set; }

        public int PeersConnected { get; set; }
        public int SeedsConnected { get; set; }

        /// <summary>
        /// integer in 1/65535ths
        /// </summary>
        public int Availability { get; set; }

        /// <summary>
        /// integer in bytes
        /// </summary>
        public long Remaining { get; set; }
    }
}