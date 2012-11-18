﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Voodoo.Basement.Routing;

namespace Voodoo.Basement.UrlConverter
{
    public class DynamicConverter : IConverter
    {
        #region 获取信息地址
        /// <summary>
        /// 获取信息地址
        /// </summary>
        /// <param name="news">信息</param>
        /// <param name="cls">栏目</param>
        /// <returns></returns>
        public string GetNewsUrl(News news, Class cls)
        {
            return string.Format("/Dynamic/News/News.aspx?id={0}", news.ID);
        }
        #endregion


        #region 获取图片内容页面地址 GetImageUrl
        /// <summary>
        /// 获取图片内容页面地址
        /// </summary>
        /// <param name="img"></param>
        /// <param name="cls"></param>
        /// <returns></returns>
        public string GetImageUrl(ImageAlbum img, Class cls)
        {
            return string.Format("/Dynamic/Image/Image.aspx?id={0}", img.ID);
        }
        #endregion

        #region 获取问答url地址
        /// <summary>
        /// 获取问答url地址
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="cls"></param>
        /// <returns></returns>
        public string GetQuestionUrl(Question qs, Class cls)
        {

            return string.Format("/Dynamic/Question/Question.aspx?id={0}", qs.ID);
        }
        #endregion

        #region 获取书籍url地址
        /// <summary>
        /// 获取问答url地址
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="cls"></param>
        /// <returns></returns>
        public string GetBookUrl(Book b, Class cls)
        {
            string url = RewriteRule.Get().BookInfo.Exp;
            url=url.Replace("{id}",b.ID.ToS());
            url=url.Replace("{classname}",b.ClassName);
            url=url.Replace("{title}",b.Title);
            url=url.Replace("{author}",b.Author);

            return url;
        }
        #endregion

        #region 获取书籍章节url地址
        /// <summary>
        /// 获取问答url地址
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="cls"></param>
        /// <returns></returns>
        public string GetBookChapterUrl(BookChapter cp, Class cls)
        {
            string url = RewriteRule.Get().BookChapter.Exp;
            url = url.Replace("{id}", cp.ID.ToS());
            url = url.Replace("{classname}", cls.ClassName);
            url = url.Replace("{title}", cp.BookTitle);
            url = url.Replace("{author}", cp.GetBook().Title);

            return url;
        }

        /// <summary>
        /// 获取章节txt文件路径
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="cls"></param>
        /// <returns></returns>
        public string GetBookChapterTxtUrl(BookChapter cp, Class cls)
        {
            using (DataEntities ent = new DataEntities())
            {
                string result = "";
                string fileName = cp.ID.ToString();

                Book b = (from l in ent.Book where l.ID == cp.BookID select l).FirstOrDefault();
                string sitrurl = "/Txt/";

                result = string.Format("{0}{1}{2}/{3}/{4}{5}",
                    sitrurl,
                    cls.ParentClassForder.IsNullOrEmpty() ? "" : cls.ParentClassForder + "/",
                    cls.ClassForder,
                     b.Title + "-" + b.Author,
                    cp.ID,
                    ".txt"
                    );
                result = Regex.Replace(result, "[/]{2,}", "/");
                return result;
            }
        }
        #endregion

        #region 获取影视地址
        /// <summary>
        /// 获取影视地址
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="cls"></param>
        /// <returns></returns>
        public string GetMovieUrl(MovieInfo b, Class cls)
        {
            return string.Format("/Dynamic/Movie/?a=content&id={0}", b.id);
        }
        #endregion

        #region 获取影视播放页面地址
        /// <summary>
        /// 获取影视地址
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="cls"></param>
        /// <returns></returns>
        public string GetMovieDramaUrl(MovieUrlBaidu b, Class cls)
        {
            return string.Format("/Dynamic/Movie/?a=baidu&id={0}", b.id);
        }

        public string GetMovieDramaUrl(MovieUrlKuaib b, Class cls)
        {
            return string.Format("/Dynamic/Movie/?a=kuaib&id={0}", b.id);
        }

        public string GetMovieDramaUrl(MovieDrama b, Class cls)
        {
            return string.Format("/Dynamic/Movie/?a=drama&id={0}", b.id);
        }

        #endregion


        #region 获取栏目地址
        /// <summary>
        /// 获取栏目地址
        /// </summary>
        /// <param name="cls">栏目</param>
        /// <returns></returns>
        public string GetClassUrl(Class cls)
        {
            string url = "";
            switch (cls.ModelID)
            {
                case 4:
                    url = RewriteRule.Get().BookClass.Exp;
                    url = url.Replace("{id}", cls.ID.ToS());
                    url = url.Replace("{classname}", cls.ClassName);
                    break;
                default:
                    url = string.Format("/Dynamic/Book/Class.aspx?id={0}", cls.ID);
                    break;
            }
            return url;
        }

        /// <summary>
        /// 获取栏目地址
        /// </summary>
        /// <param name="cls"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public string GetClassUrl(Class cls, int page)
        {
            return string.Format("/Dynamic/Book/Class.aspx?id={0}&page={1}", cls.ID,page);
        }
        #endregion
    }
}