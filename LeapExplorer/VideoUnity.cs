using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Declarations.Media;
using Declarations.Players;
using Implementation;

namespace LeapExplorer
{
    internal class VideoUnity
    {
        /// <summary>
        /// 截取视频缩略图
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="imgFile"></param>
        /// <returns></returns>
        public static bool CatchImg(string fileName, string imgFile)
        {
            const string ffmpeg = "ffmpeg.exe";
            //string flvImg = imgFile + ".jpg";
            string flvImgSize = "640*480";
            MediaPlayerFactory m_factory = new MediaPlayerFactory();
            IVideoPlayer m_player = m_factory.CreatePlayer<IVideoPlayer>();
            IMediaFromFile m_media = m_factory.CreateMedia<IMediaFromFile>(fileName);
            m_player.Open(m_media);
            m_media.Parse(true);

            System.Drawing.Size size = m_player.GetVideoSize(0);
            if (!size.IsEmpty)
                flvImgSize = size.Width.ToString() + "*" + size.Height.ToString();
            //m_player.TakeSnapShot(1, @"C:");
            System.Diagnostics.ProcessStartInfo ImgstartInfo = new System.Diagnostics.ProcessStartInfo(ffmpeg);
            ImgstartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            ImgstartInfo.Arguments = "   -i   " + fileName + "  -y  -f  image2   -ss 2 -vframes 1  -s   " + flvImgSize +
                                     "   " + imgFile;
            try
            {
                System.Diagnostics.Process.Start(ImgstartInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
    }
}