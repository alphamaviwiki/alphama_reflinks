using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Cache;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Collections.Specialized;
using System.Reflection;
using System.Xml;
using System.Xml.Xsl;
using System.Diagnostics;
using System.Threading;

namespace AlphamaConverter
{
    public partial class Converter : Form
    {
        private ToolStripMenuItem options1;
        private ToolStripMenuItem options2;
        List<string> metatags = new List<string>();
        List<string> refs = new List<string>();
        List<string> convertedrefs = new List<string>();

        public Converter()
        {
            InitializeComponent();

            options1 = this.convertAllRefsExceptchúThíchTemplateToolStripMenuItem;
            options2 = this.convertAllToolStripMenuItem;
            comboBox2.SelectedIndex = 0;
        }

        public Converter(string input, ref string output)
        {
            InitializeComponent();
            this.textBox1.Text = input;
            Execute();
            output = this.textBox2.Text;
            
        }

        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public void GetRefs(string text)
        {

            refs = new List<string>();
            convertedrefs = new List<string>();
            string Check = text;
            string result = String.Empty;

            while (true)
            {
                Match m = Regex.Match(Check, @"<ref");
                Match m1 = Regex.Match(Check, @"</ref>");
                Match m2 = Regex.Match(Check, @"/>");

                int tempm2 = m2.Index;
                if (m.Success && m1.Success)
                {

                    if (m2.Index == 0) tempm2 = m1.Index + 1;
                    if (m1.Index > m.Index && tempm2 > m1.Index)
                    {
                        string s = Check.Substring(m.Index, m1.Index + m1.Length - m.Index);

                        //if (CheckRef(s) == false)
                        if (options2.Checked)
                        {
                            refs.Add(s);
                        }
                        else if (CheckRef(s) == false)
                        {

                            refs.Add(s);
                        }

                        Check = Check.Substring(m1.Index + m1.Length, Check.Length - m1.Index - m1.Length);
                    }
                    else Check = Check.Substring(m2.Index + m2.Length, Check.Length - m2.Index - m2.Length);
                }
                else break;
            }

        }

        public void FormatRepeatRefs()
        {
            for (int i = 0; i < convertedrefs.Count; i++)
            {
                bool repeat = false;

                for (int j = i + 1; j < convertedrefs.Count; j++)
                {
                    if (convertedrefs[i] == convertedrefs[j] && !convertedrefs[i].Contains("<ref name=\"source") && !convertedrefs[i].Contains("\"/>"))
                    {
                        convertedrefs[j] = "<ref name=\"source" + i + "\"/>";
                        repeat = true;
                    }
                }

                if (repeat)
                {
                    Match mref = Regex.Match(convertedrefs[i], @"<ref");

                    Match mref1 = Regex.Match(convertedrefs[i], @"name");

                    if (mref.Success && !mref1.Success)
                    {
                        convertedrefs[i] = convertedrefs[i].Insert(mref.Index + mref.Length, " name=\"source" + i + "\"");
                    }

                    else
                    {
                        convertedrefs[i] = "<ref name=\"source" + i + "\">" + convertedrefs[i].Substring(convertedrefs[i].IndexOf(">")+1);
                    }
                }
            }

        }

        public bool CheckRef(string text)
        {
            bool result = false;
            Match m = Regex.Match(text, @"\{\{\s*[Cc]hú\s*[Tt]hích");
            Match m1 = Regex.Match(text, @"(http|https)://");
            Match m2 = Regex.Match(text, @"{{chú thích");
            Match m3 = Regex.Match(text, @"\[");


            if (!m1.Success) result = true;
            if (m.Success) result = true;
            if (m2.Success) result = true;

            if (!options1.Checked)
            {

                if (m3.Success) result = true;
            }

            return result;
        }

        public bool CheckErrorContent(string text)
        {
            bool check = false;

            Match m0 = Regex.Match(text, @"[Pp]age\s*[Nn]ot\s*[Ff]ound");
            Match m1 = Regex.Match(text, @"[Kk]hông\s*tìm\s*thấy.*đường\s*dẫn\s*này");
            Match m2 = Regex.Match(text, @"[Bb]ạn có thể truy cập vào.*trang chủ.*hoặc sử dụng ô dưới đây để tìm kiếm");
            Match m3 = Regex.Match(text, @"Không tìm thấy nội dung này");
            Match m4 = Regex.Match(text, @"Không tìm thấy bài viết tại địa chỉ");

            if (text.Length == 0) check = true;
            if (m0.Success) check = true;
            if (m1.Success) check = true;
            if (m2.Success) check = true;
            if (m3.Success) check = true;
            if (m4.Success) check = true;

            return check;
        }

        public bool CheckRedirect(string text)
        {
            bool check = false;
            Match m0 = Regex.Match(text, @"Redirect...");
            if (m0.Success) check = true;
            return check;
        }

        public bool CheckRefRepetition(string item, int index)
        {
            bool checkRepeat = false;
            for (int j = 0; j < index; j++)
            {

                if (item == refs[j])
                {
                    convertedrefs.Add(convertedrefs[j]);
                    checkRepeat = true;
                }

                if (checkRepeat == true) break;
            }
            return checkRepeat;
        }

        public string RemoveFirstRefTag(string item)
        {
            
            Match mname = Regex.Match(item, @">");
            string tempurl = String.Empty;
            if (mname.Success) tempurl = item.Substring(mname.Index + 1);
            else tempurl = item;

            return tempurl;
           
        }

        public string GetTitle(string strContent, string url)
        {
            string title = String.Empty;
            try
            {
                Match mtitlebegin = Regex.Match(strContent, "<title");
                Match mtitleend = Regex.Match(strContent, "</title");

                if (mtitlebegin.Success && mtitleend.Success)
                {
                    title = strContent.Substring(mtitlebegin.Index, mtitleend.Index - mtitlebegin.Index);
                    title = title.Substring(title.IndexOf(">") + 1);
                    title = WebUtility.HtmlDecode(title);
                    title = title.Replace("&nbsp;", " ");
                    title = title.Replace("\r\n", "");
                    title = title.Replace("\r", "");
                    title = title.Replace("\n", "");
                    title = title.Replace("“", "\"");
                    title = title.Replace("”", "\"");

                    if (url.Contains("nhandan.com.vn"))
                    {
                        title = title.Substring(title.LastIndexOf("-") + 1);
                    }
                    
                    if (url.Contains("sggp.org.vn"))
                    {
                        title = title.Substring(title.IndexOf("-") + 1);
                    }

                    if (url.Contains("baodientu.chinhphu.vn"))
                    {
                        title = title.Substring(title.IndexOf("|") + 1, title.LastIndexOf("|") - title.IndexOf("|") - 1);
                    }

                    if (url.Contains("cand.com.vn"))
                    {
                        title = title.Substring(title.IndexOf("|") + 1, title.LastIndexOf("-") - title.IndexOf("|") - 1);
                    }

                    if (url.Contains("giadinh.net.vn"))
                    {
                        title = title.Substring(0, title.IndexOf("|"));
                    }

                    // Không chứa gì cả
                    if (url.Contains("baocamau.com.vn"))
                    {
                        title = "";
                    }

                    if (url.Contains("vnmedia.vn"))
                    {
                        title = title.Substring(title.LastIndexOf("/") + 1);
                    }

                    if (url.Contains("giacngo.vn"))
                    {
                        title = title.Substring(title.LastIndexOf("-") + 1);
                    }

                    if (url.Contains("baoanhdatmui.vn"))
                    {
                        title = title.Substring(title.IndexOf(":") + 1, title.IndexOf("|"));
                    }

                    if ((url.Contains("vov.vn") || url.Contains("afamily.vn") || url.Contains("24h.com.vn") || url.Contains("news.go.vn") || url.Contains("nld.com.vn")))
                    {

                        title = title.Substring(0, title.IndexOf("|"));
                    }

                    // Most cases
                    if (title != "" && title.Contains("-")) title = title.Substring(0, title.IndexOf("-"));
                    if (title != "" && title.Contains("|")) title = title.Substring(0, title.IndexOf("|"));
                   
                    title = title.Replace("|", "");
                }
            }
            catch
            {
                title = "";
            }

            return title;
           
        }

        public bool GetMetaTags(string re, string strContent)
        {
            bool check = false;
            string strTemp = strContent;
            Match metabegin = Regex.Match(strTemp, @"<meta");
            if (metabegin.Success) strTemp = strTemp.Substring(metabegin.Index, strTemp.Length - metabegin.Index);
            else
            {
                convertedrefs.Add(re);
                check = true;
                return check;
            }

            while (true)
            {

                metabegin = Regex.Match(strTemp, @"<meta");
                Match metaclose = Regex.Match(strTemp, @"/>");

                if (metabegin.Success)
                {
                    if (metaclose.Success && metaclose.Index > metabegin.Index)
                    {
                        //Cần kiểm tra thẻ meta đóng
                        string item = strTemp.Substring(metabegin.Index, metaclose.Index + metaclose.Length - metabegin.Index);
                        if (Regex.Matches(item, "<meta").Count > 1)
                        {
                            item = item.Substring(item.LastIndexOf("<meta"));
                        }
                        metatags.Add(item);
                        strTemp = strTemp.Substring(metaclose.Index + metaclose.Length, strTemp.Length - metaclose.Index - metaclose.Length);
                    }
                    else
                    {
                        if (metaclose.Index == 0) break;
                        strTemp = strTemp.Substring(metaclose.Index + metaclose.Length, strTemp.Length - metaclose.Index - metaclose.Length);
                    }

                }
                else break;
            }
            return check;
           
        }

        public void CreateRef(URLInfo ui, string title, string re)
        {
            
            string resultRef = String.Empty;
            if (ui.UrlTitle == "" | ui.UrlTitle == null)
            {
                if (title != "")
                {
                    ui.UrlTitle = title;
                }

               
            }

            if (ui.UrlTitle != "" && ui.UrlTitle != null)
            {

                //Get Infor from Sources
                URLInfo utemp = new URLInfo();
                utemp.LoadURL();

                utemp = utemp.SearchbyURLink(ui.UrlLink);
                if (utemp != null)
                {
                    ui.Language = utemp.Language;
                    ui.SiteName = utemp.SiteName;
                }

                //Bỏ tiếng Việt
                if (ui.Language == "Tiếng Việt") ui.Language = "";



                if (this.textBox4.Text == "en")
                {
                    
                    resultRef += "{{cite web | url = " + ui.UrlLink + " | title = " + ui.UrlTitle + " | author = " + ui.Author + " | date = " + ui.DatePublished + " | accessdate = " + ui.DateRetrieved + " | publisher = " + ui.SiteName + " | language = " + ui.Language + "}}";
                }
                else
                {
                    resultRef += "{{chú thích web | url = " + ui.UrlLink + " | tiêu đề = " + ui.UrlTitle + " | author = " + ui.Author + " | ngày = " + ui.DatePublished + " | ngày truy cập = " + ui.DateRetrieved + " | nơi xuất bản = " + ui.SiteName + " | ngôn ngữ = " + ui.Language + "}}";
                }

                //Nếu chứa thẻ name thì thêm vào

                string tempref = String.Empty;

                if (re.Contains("name"))
                {
                    tempref = re.Substring(0, re.IndexOf(">") + 1);

                    convertedrefs.Add(tempref + resultRef + "</ref>");
                }
                else convertedrefs.Add("<ref>" + resultRef + "</ref>");


            }
            else
            {

                convertedrefs.Add(re);
            }
            
        }

        public void ConvertText()
        {
            for (int i = 0; i < refs.Count; i++)
            {
                string re = refs[i]; metatags = new List<string>();
                if (CheckRefRepetition(re, i) == true)
                {
                    progressBar1.PerformStep(); continue;
                }
                
               
                if (options2.Checked)
                {
                    Match url = Regex.Match(RemoveFirstRefTag(re), @"(?i)\b((?:https?:(?:/{1,3}|[a-z0-9%])|[a-z0-9.\-]+[.](?:com|net|org|edu|gov|mil|aero|asia|biz|cat|coop|info|int|jobs|mobi|museum|name|post|pro|tel|travel|xxx|ac|ad|ae|af|ag|ai|al|am|an|ao|aq|ar|as|at|au|aw|ax|az|ba|bb|bd|be|bf|bg|bh|bi|bj|bm|bn|bo|br|bs|bt|bv|bw|by|bz|ca|cc|cd|cf|cg|ch|ci|ck|cl|cm|cn|co|cr|cs|cu|cv|cx|cy|cz|dd|de|dj|dk|dm|do|dz|ec|ee|eg|eh|er|es|et|eu|fi|fj|fk|fm|fo|fr|ga|gb|gd|ge|gf|gg|gh|gi|gl|gm|gn|gp|gq|gr|gs|gt|gu|gw|gy|hk|hm|hn|hr|ht|hu|id|ie|il|im|in|io|iq|ir|is|it|je|jm|jo|jp|ke|kg|kh|ki|km|kn|kp|kr|kw|ky|kz|la|lb|lc|li|lk|lr|ls|lt|lu|lv|ly|ma|mc|md|me|mg|mh|mk|ml|mm|mn|mo|mp|mq|mr|ms|mt|mu|mv|mw|mx|my|mz|na|nc|ne|nf|ng|ni|nl|no|np|nr|nu|nz|om|pa|pe|pf|pg|ph|pk|pl|pm|pn|pr|ps|pt|pw|py|qa|re|ro|rs|ru|rw|sa|sb|sc|sd|se|sg|sh|si|sj|Ja|sk|sl|sm|sn|so|sr|ss|st|su|sv|sx|sy|sz|tc|td|tf|tg|th|tj|tk|tl|tm|tn|to|tp|tr|tt|tv|tw|tz|ua|ug|uk|us|uy|uz|va|vc|ve|vg|vi|vn|vu|wf|ws|ye|yt|yu|za|zm|zw)/)(?:[^\s()<>{}\[\]]+|\([^\s()]*?\([^\s()]+\)[^\s()]*?\)|\([^\s]+?\))+(?:\([^\s()]*?\([^\s()]+\)[^\s()]*?\)|\([^\s]+?\)|[^\s`!()\[\]{};:'"".,<>?«»“”‘’])|(?:(?<!@)[a-z0-9]+(?:[.\-][a-z0-9]+)*[.](?:com|net|org|edu|gov|mil|aero|asia|biz|cat|coop|info|int|jobs|mobi|museum|name|post|pro|tel|travel|xxx|ac|ad|ae|af|ag|ai|al|am|an|ao|aq|ar|as|at|au|aw|ax|az|ba|bb|bd|be|bf|bg|bh|bi|bj|bm|bn|bo|br|bs|bt|bv|bw|by|bz|ca|cc|cd|cf|cg|ch|ci|ck|cl|cm|cn|co|cr|cs|cu|cv|cx|cy|cz|dd|de|dj|dk|dm|do|dz|ec|ee|eg|eh|er|es|et|eu|fi|fj|fk|fm|fo|fr|ga|gb|gd|ge|gf|gg|gh|gi|gl|gm|gn|gp|gq|gr|gs|gt|gu|gw|gy|hk|hm|hn|hr|ht|hu|id|ie|il|im|in|io|iq|ir|is|it|je|jm|jo|jp|ke|kg|kh|ki|km|kn|kp|kr|kw|ky|kz|la|lb|lc|li|lk|lr|ls|lt|lu|lv|ly|ma|mc|md|me|mg|mh|mk|ml|mm|mn|mo|mp|mq|mr|ms|mt|mu|mv|mw|mx|my|mz|na|nc|ne|nf|ng|ni|nl|no|np|nr|nu|nz|om|pa|pe|pf|pg|ph|pk|pl|pm|pn|pr|ps|pt|pw|py|qa|re|ro|rs|ru|rw|sa|sb|sc|sd|se|sg|sh|si|sj|Ja|sk|sl|sm|sn|so|sr|ss|st|su|sv|sx|sy|sz|tc|td|tf|tg|th|tj|tk|tl|tm|tn|to|tp|tr|tt|tv|tw|tz|ua|ug|uk|us|uy|uz|va|vc|ve|vg|vi|vn|vu|wf|ws|ye|yt|yu|za|zm|zw)\b/?(?!@)))");
                    if (url.Success)
                    {
                        
                        URLInfo ui = new URLInfo(); ui.UrlLink = url.Value;
                        // Try get data 3 times
                        string strContent = GetWebData(url.Value);
                        if (CheckErrorContent(strContent) == true) strContent = GetWebData(url.Value);
                        //if (CheckErrorContent(strContent) == true) strContent = GetWebData(url.Value);
                        if (CheckErrorContent(strContent) == true) strContent = "";

                        //if (url.Value.Contains("dantri.com.vn"))
                        //{
                        //    while (strContent == "(502) Bad Gateway")
                        //    {
                        //        GetWebData(url.Value);
                        //    }
                        //}

                        if (strContent == "(502) Bad Gateway")
                        {
                            Match m = Regex.Match(re, "</ref>");
                            if (m.Success)
                            {
                                re = re.Insert(m.Index, " <!-- lỗi server -->"); convertedrefs.Add(re);
                            }
                            progressBar1.PerformStep(); 
                            continue;
                        }

                        if (strContent == "Liên kết lỗi 123")
                        {
                            Match m = Regex.Match(re, "</ref>");
                            if (m.Success)
                            {
                                re = re.Insert(m.Index, " <!-- liên kết lỗi -->"); convertedrefs.Add(re);
                            }
                            progressBar1.PerformStep();
                            continue;
                        }
                    
                        if (strContent != "" && strContent != null)
                        {
                            string title = GetTitle(strContent, re);
                            if (GetMetaTags(re, strContent) == true) continue;
                          
                            #region Get information from meta tags
                            foreach (string meta in metatags)
                            {
                                //trường hợp lỗi 2 thẻ meta cùng lúc do 1 thẻ không có thẻ đóng />
                                //if (Regex.Matches("meta", "true").Count > 2) continue;

                                Match ogsitename = Regex.Match(meta, @"og:site_name");
                                Match itempropdatepublished = Regex.Match(meta, @"itemprop=""datePublished""");
                                Match inlanguage = Regex.Match(meta, @"itemprop=""inLanguage""");
                                Match itempropauthor = Regex.Match(meta, @"itemprop=""author""");
                                Match ogtitle = Regex.Match(meta, @"og:title");
                                Match content = Regex.Match(meta, @"content=""([^""]*)""");
                                //Match ogurl = Regex.Match(meta, @"og:url");

                                // Alternative matching

                                #region Sitename
                                if (ogsitename.Success && content.Success)
                                {
                                    try
                                    {
                                        ui.SiteName = content.Value.Substring(content.Value.IndexOf("\"") + 1, content.Value.Length - content.Value.IndexOf("\"") - 2);


                                        //if (re.Contains("vir.com.vn"))
                                        //{
                                        //    ui.SiteName = ui.SiteName.Substring(0, ui.SiteName.IndexOf("|"));
                                        //}

                                        ui.SiteName = ui.SiteName.Replace("|", "");



                                    }
                                    catch
                                    {
                                        ui.SiteName = "";
                                    }


                                    if (ui.SiteName == "")
                                    {
                                        try
                                        {
                                            // Lấy title của trang
                                            ui.SiteName = title.Substring(0, title.IndexOf("-"));
                                            ui.SiteName = WebUtility.HtmlDecode(ui.SiteName);
                                            ui.SiteName = ui.SiteName.Replace("&nbsp;", " ");
                                            ui.SiteName = ui.SiteName.Replace("|", "");



                                        }

                                        catch
                                        {
                                            ui.SiteName = "";
                                        }

                                    }

                                    ui.SiteName = ui.SiteName.Trim();

                                }
                                #endregion

                                #region UrlTitle
                                if (ogtitle.Success && content.Success)
                                {
                                    try
                                    {
                                        ui.UrlTitle = content.Value.Substring(content.Value.IndexOf("\"") + 1, content.Value.Length - content.Value.IndexOf("\"") - 2);
                                        ui.UrlTitle = WebUtility.HtmlDecode(ui.UrlTitle);
                                        ui.UrlTitle = ui.UrlTitle.Replace("&nbsp;", " ");
                                        ui.UrlTitle = ui.UrlTitle.Replace("|", "");

                                    }
                                    catch
                                    {
                                        ui.UrlTitle = "";
                                    }
                                    ui.UrlTitle = ui.UrlTitle.Trim();
                                }
                                #endregion

                                #region DatePublished
                                if (itempropdatepublished.Success && content.Success)
                                {
                                    DateTime tempdate = new DateTime();
                                    try
                                    {
                                        tempdate = Convert.ToDateTime(content.Value.Substring(content.Value.IndexOf("\"") + 1, content.Value.Length - content.Value.IndexOf("\"") - 2));

                                        if (this.textBox4.Text == "en") ui.DateRetrieved = tempdate.ToString("MMMM dd, yyyy");
                                        else ui.DatePublished = tempdate.Day + " tháng " + tempdate.Month + " năm " + tempdate.Year;

                                    }
                                    catch
                                    {
                                        ui.DatePublished = "";
                                    }
                                }

                                #region DateRetrieved

                                if (this.textBox4.Text == "en") ui.DateRetrieved = DateTime.Now.ToString("MMMM dd, yyyy");
                                else ui.DateRetrieved = DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
                                #endregion
                                #endregion



                                #region Language
                                if (inlanguage.Success && content.Success)
                                {
                                    try
                                    {
                                        ui.Language = content.Value.Substring(content.Value.IndexOf("\"") + 1, content.Value.Length - content.Value.IndexOf("\"") - 2);
                                        if (ui.Language.Contains("vi")) ui.Language = "Tiếng Việt";
                                        if (ui.Language.Contains("en")) ui.Language = "Tiếng Anh";
                                        if (ui.Language.Contains("fr")) ui.Language = "Tiếng Pháp";
                                        if (ui.Language.Contains("de")) ui.Language = "Tiếng Đức";
                                        if (ui.Language.Contains("ja")) ui.Language = "Tiếng Nhật";
                                        if (ui.Language.Contains("zh")) ui.Language = "Tiếng Trung Quốc";
                                        if (ui.Language.Contains("ko")) ui.Language = "Tiếng Hàn Quốc";
                                        if (ui.Language.Contains("ru")) ui.Language = "Tiếng Nga";
                                        if (ui.Language.Contains("th")) ui.Language = "Tiếng Thái Lan";
                                        if (ui.Language.Contains("es")) ui.Language = "Tiếng Tây Ban Nha";
                                        if (ui.Language.Contains("pt")) ui.Language = "Tiếng Bồ Đào Nha";
                                        if (ui.Language.Contains("it")) ui.Language = "Tiếng Ý";
                                        if (ui.Language.Contains("ar")) ui.Language = "Tiếng Ả Rập";
                                    }
                                    catch
                                    {
                                        ui.Language = "";
                                    }

                                    //Tạm để trống giá trị của tham số này
                                    ui.Language = "";

                                    ui.Language = ui.Language.Trim();
                                }
                                #endregion

                                #region Author
                                if (itempropauthor.Success && content.Success)
                                {
                                    try
                                    {
                                        ui.Author = content.Value.Substring(content.Value.IndexOf("\"") + 1, content.Value.Length - content.Value.IndexOf("\"") - 2);
                                        ui.Author = WebUtility.HtmlDecode(ui.Author);
                                        ui.Author = ui.Author.Replace("&nbsp;", " ");
                                    }
                                    catch
                                    {
                                        ui.Author = "";
                                    }
                                    ui.Author = ui.Author.Trim();
                                }
                                #endregion


                            }
                            #endregion

                            CreateRef(ui, title, re);
                        }
                        else
                        {
                            progressBar1.PerformStep();
                            #region Add link die template
                            Match m = Regex.Match(re, "</ref>");
                            if (m.Success)
                            {
                                re = re.Insert(m.Index, " {{link chết|truy vấn quá lâu}}");
                                convertedrefs.Add(re);
                            }
                            #endregion
                            else convertedrefs.Add(re);
                            continue;
                        }
                    }

                }
                else if (!CheckRef(re))
                {
                    Match url = Regex.Match(RemoveFirstRefTag(re), @"(?i)\b((?:https?:(?:/{1,3}|[a-z0-9%])|[a-z0-9.\-]+[.](?:com|net|org|edu|gov|mil|aero|asia|biz|cat|coop|info|int|jobs|mobi|museum|name|post|pro|tel|travel|xxx|ac|ad|ae|af|ag|ai|al|am|an|ao|aq|ar|as|at|au|aw|ax|az|ba|bb|bd|be|bf|bg|bh|bi|bj|bm|bn|bo|br|bs|bt|bv|bw|by|bz|ca|cc|cd|cf|cg|ch|ci|ck|cl|cm|cn|co|cr|cs|cu|cv|cx|cy|cz|dd|de|dj|dk|dm|do|dz|ec|ee|eg|eh|er|es|et|eu|fi|fj|fk|fm|fo|fr|ga|gb|gd|ge|gf|gg|gh|gi|gl|gm|gn|gp|gq|gr|gs|gt|gu|gw|gy|hk|hm|hn|hr|ht|hu|id|ie|il|im|in|io|iq|ir|is|it|je|jm|jo|jp|ke|kg|kh|ki|km|kn|kp|kr|kw|ky|kz|la|lb|lc|li|lk|lr|ls|lt|lu|lv|ly|ma|mc|md|me|mg|mh|mk|ml|mm|mn|mo|mp|mq|mr|ms|mt|mu|mv|mw|mx|my|mz|na|nc|ne|nf|ng|ni|nl|no|np|nr|nu|nz|om|pa|pe|pf|pg|ph|pk|pl|pm|pn|pr|ps|pt|pw|py|qa|re|ro|rs|ru|rw|sa|sb|sc|sd|se|sg|sh|si|sj|Ja|sk|sl|sm|sn|so|sr|ss|st|su|sv|sx|sy|sz|tc|td|tf|tg|th|tj|tk|tl|tm|tn|to|tp|tr|tt|tv|tw|tz|ua|ug|uk|us|uy|uz|va|vc|ve|vg|vi|vn|vu|wf|ws|ye|yt|yu|za|zm|zw)/)(?:[^\s()<>{}\[\]]+|\([^\s()]*?\([^\s()]+\)[^\s()]*?\)|\([^\s]+?\))+(?:\([^\s()]*?\([^\s()]+\)[^\s()]*?\)|\([^\s]+?\)|[^\s`!()\[\]{};:'"".,<>?«»“”‘’])|(?:(?<!@)[a-z0-9]+(?:[.\-][a-z0-9]+)*[.](?:com|net|org|edu|gov|mil|aero|asia|biz|cat|coop|info|int|jobs|mobi|museum|name|post|pro|tel|travel|xxx|ac|ad|ae|af|ag|ai|al|am|an|ao|aq|ar|as|at|au|aw|ax|az|ba|bb|bd|be|bf|bg|bh|bi|bj|bm|bn|bo|br|bs|bt|bv|bw|by|bz|ca|cc|cd|cf|cg|ch|ci|ck|cl|cm|cn|co|cr|cs|cu|cv|cx|cy|cz|dd|de|dj|dk|dm|do|dz|ec|ee|eg|eh|er|es|et|eu|fi|fj|fk|fm|fo|fr|ga|gb|gd|ge|gf|gg|gh|gi|gl|gm|gn|gp|gq|gr|gs|gt|gu|gw|gy|hk|hm|hn|hr|ht|hu|id|ie|il|im|in|io|iq|ir|is|it|je|jm|jo|jp|ke|kg|kh|ki|km|kn|kp|kr|kw|ky|kz|la|lb|lc|li|lk|lr|ls|lt|lu|lv|ly|ma|mc|md|me|mg|mh|mk|ml|mm|mn|mo|mp|mq|mr|ms|mt|mu|mv|mw|mx|my|mz|na|nc|ne|nf|ng|ni|nl|no|np|nr|nu|nz|om|pa|pe|pf|pg|ph|pk|pl|pm|pn|pr|ps|pt|pw|py|qa|re|ro|rs|ru|rw|sa|sb|sc|sd|se|sg|sh|si|sj|Ja|sk|sl|sm|sn|so|sr|ss|st|su|sv|sx|sy|sz|tc|td|tf|tg|th|tj|tk|tl|tm|tn|to|tp|tr|tt|tv|tw|tz|ua|ug|uk|us|uy|uz|va|vc|ve|vg|vi|vn|vu|wf|ws|ye|yt|yu|za|zm|zw)\b/?(?!@)))");
                    if (url.Success)
                    {

                        URLInfo ui = new URLInfo(); ui.UrlLink = url.Value;
                        // Try get data 3 times
                        string strContent = GetWebData(url.Value);
                        if (CheckErrorContent(strContent) == true) strContent = GetWebData(url.Value);
                        //if (CheckErrorContent(strContent) == true) strContent = GetWebData(url.Value);
                        if (CheckErrorContent(strContent) == true) strContent = "";

                        //if (url.Value.Contains("dantri.com.vn"))
                        //{
                        //    while (strContent == "(502) Bad Gateway")
                        //    {
                        //        GetWebData(url.Value);
                        //    }
                        //}

                        if (strContent == "(502) Bad Gateway")
                        {
                            Match m = Regex.Match(re, "</ref>");
                            if (m.Success)
                            {
                                re = re.Insert(m.Index, " <!-- lỗi server -->"); convertedrefs.Add(re);
                            }
                            progressBar1.PerformStep();
                            continue;
                        }

                        if (strContent == "Liên kết lỗi 123")
                        {
                            Match m = Regex.Match(re, "</ref>");
                            if (m.Success)
                            {
                                re = re.Insert(m.Index, " <!-- liên kết lỗi -->"); convertedrefs.Add(re);
                            }
                            progressBar1.PerformStep();
                            continue;
                        }

                        if (strContent != "" && strContent != null)
                        {
                            string title = GetTitle(strContent, re);
                            if (GetMetaTags(re, strContent) == true) continue;

                            #region Get information from meta tags
                            foreach (string meta in metatags)
                            {
                                //trường hợp lỗi 2 thẻ meta cùng lúc do 1 thẻ không có thẻ đóng />
                                //if (Regex.Matches("meta", "true").Count > 2) continue;

                                Match ogsitename = Regex.Match(meta, @"og:site_name");
                                Match itempropdatepublished = Regex.Match(meta, @"itemprop=""datePublished""");
                                Match inlanguage = Regex.Match(meta, @"itemprop=""inLanguage""");
                                Match itempropauthor = Regex.Match(meta, @"itemprop=""author""");
                                Match ogtitle = Regex.Match(meta, @"og:title");
                                Match content = Regex.Match(meta, @"content=""([^""]*)""");
                                //Match ogurl = Regex.Match(meta, @"og:url");

                                // Alternative matching

                                #region Sitename
                                if (ogsitename.Success && content.Success)
                                {
                                    try
                                    {
                                        ui.SiteName = content.Value.Substring(content.Value.IndexOf("\"") + 1, content.Value.Length - content.Value.IndexOf("\"") - 2);


                                        //if (re.Contains("vir.com.vn"))
                                        //{
                                        //    ui.SiteName = ui.SiteName.Substring(0, ui.SiteName.IndexOf("|"));
                                        //}

                                        ui.SiteName = ui.SiteName.Replace("|", "");



                                    }
                                    catch
                                    {
                                        ui.SiteName = "";
                                    }


                                    if (ui.SiteName == "")
                                    {
                                        try
                                        {
                                            // Lấy title của trang
                                            ui.SiteName = title.Substring(0, title.IndexOf("-"));
                                            ui.SiteName = WebUtility.HtmlDecode(ui.SiteName);
                                            ui.SiteName = ui.SiteName.Replace("&nbsp;", " ");
                                            ui.SiteName = ui.SiteName.Replace("|", "");



                                        }

                                        catch
                                        {
                                            ui.SiteName = "";
                                        }

                                    }

                                    ui.SiteName = ui.SiteName.Trim();

                                }
                                #endregion

                                #region UrlTitle
                                if (ogtitle.Success && content.Success)
                                {
                                    try
                                    {
                                        ui.UrlTitle = content.Value.Substring(content.Value.IndexOf("\"") + 1, content.Value.Length - content.Value.IndexOf("\"") - 2);
                                        ui.UrlTitle = WebUtility.HtmlDecode(ui.UrlTitle);
                                        ui.UrlTitle = ui.UrlTitle.Replace("&nbsp;", " ");
                                        ui.UrlTitle = ui.UrlTitle.Replace("|", "");

                                    }
                                    catch
                                    {
                                        ui.UrlTitle = "";
                                    }
                                    ui.UrlTitle = ui.UrlTitle.Trim();
                                }
                                #endregion

                                #region DatePublished
                                if (itempropdatepublished.Success && content.Success)
                                {
                                    DateTime tempdate = new DateTime();
                                    try
                                    {
                                        tempdate = Convert.ToDateTime(content.Value.Substring(content.Value.IndexOf("\"") + 1, content.Value.Length - content.Value.IndexOf("\"") - 2));

                                        if (this.textBox4.Text == "en") ui.DateRetrieved = tempdate.ToString("MMMM dd, yyyy");
                                        else ui.DatePublished = tempdate.Day + " tháng " + tempdate.Month + " năm " + tempdate.Year;

                                    }
                                    catch
                                    {
                                        ui.DatePublished = "";
                                    }
                                }

                                #region DateRetrieved

                                if (this.textBox4.Text == "en") ui.DateRetrieved = DateTime.Now.ToString("MMMM dd, yyyy");
                                else ui.DateRetrieved = DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
                                #endregion
                                #endregion



                                #region Language
                                if (inlanguage.Success && content.Success)
                                {
                                    try
                                    {
                                        ui.Language = content.Value.Substring(content.Value.IndexOf("\"") + 1, content.Value.Length - content.Value.IndexOf("\"") - 2);
                                        if (ui.Language.Contains("vi")) ui.Language = "Tiếng Việt";
                                        if (ui.Language.Contains("en")) ui.Language = "Tiếng Anh";
                                        if (ui.Language.Contains("fr")) ui.Language = "Tiếng Pháp";
                                        if (ui.Language.Contains("de")) ui.Language = "Tiếng Đức";
                                        if (ui.Language.Contains("ja")) ui.Language = "Tiếng Nhật";
                                        if (ui.Language.Contains("zh")) ui.Language = "Tiếng Trung Quốc";
                                        if (ui.Language.Contains("ko")) ui.Language = "Tiếng Hàn Quốc";
                                        if (ui.Language.Contains("ru")) ui.Language = "Tiếng Nga";
                                        if (ui.Language.Contains("th")) ui.Language = "Tiếng Thái Lan";
                                        if (ui.Language.Contains("es")) ui.Language = "Tiếng Tây Ban Nha";
                                        if (ui.Language.Contains("pt")) ui.Language = "Tiếng Bồ Đào Nha";
                                        if (ui.Language.Contains("it")) ui.Language = "Tiếng Ý";
                                        if (ui.Language.Contains("ar")) ui.Language = "Tiếng Ả Rập";
                                    }
                                    catch
                                    {
                                        ui.Language = "";
                                    }

                                    //Tạm để trống giá trị của tham số này
                                    ui.Language = "";

                                    ui.Language = ui.Language.Trim();
                                }
                                #endregion

                                #region Author
                                if (itempropauthor.Success && content.Success)
                                {
                                    try
                                    {
                                        ui.Author = content.Value.Substring(content.Value.IndexOf("\"") + 1, content.Value.Length - content.Value.IndexOf("\"") - 2);
                                        ui.Author = WebUtility.HtmlDecode(ui.Author);
                                        ui.Author = ui.Author.Replace("&nbsp;", " ");
                                    }
                                    catch
                                    {
                                        ui.Author = "";
                                    }
                                    ui.Author = ui.Author.Trim();
                                }
                                #endregion


                            }
                            #endregion

                            CreateRef(ui, title, re);
                        }
                        else
                        {
                            progressBar1.PerformStep();
                            #region Add link die template
                            Match m = Regex.Match(re, "</ref>");
                            if (m.Success)
                            {
                                re = re.Insert(m.Index, " {{link chết|truy vấn quá lâu}}");
                                convertedrefs.Add(re);
                            }
                            #endregion
                            else convertedrefs.Add(re);
                            continue;
                        }
                    }
                }
                    
                else  convertedrefs.Add(re);
                progressBar1.PerformStep();
            }

        }

        public static string GetWebData(string link)
        {
            string result = String.Empty;
            try
            {
                Uri uri = null;
                try
                {
                    uri = new Uri(link);
                }
                catch
                {
                    return "Liên kết lỗi 123";
                    
                }

                HttpWebRequest objWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                
               
                objWebRequest.Timeout = 60000;
                objWebRequest.Credentials = CredentialCache.DefaultCredentials;
                objWebRequest.Proxy = GlobalProxySelection.GetEmptyWebProxy();
                //objWebRequest.Method = "POST";
                //objWebRequest.ContentType = "application/x-www-form-urlencoded";


                objWebRequest.AllowAutoRedirect = false;
                objWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.114 Safari/537.36";
                //objWebRequest.UserAgent = "Google Bot";
                objWebRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);

                HttpWebResponse objWebResponse;
                using (objWebResponse = (HttpWebResponse)objWebRequest.GetResponse())
                {
                    //if ((int)objWebResponse.StatusCode >= 300 && (int)objWebResponse.StatusCode <= 399)
                    //{
                    //    string uriString = objWebResponse.Headers["Location"];
                    //    objWebResponse.Close();

                    //    result = GetWebData(uriString);
                    //}

                   

                    if ((int)objWebResponse.StatusCode == 200)
                    {
                        Stream receiveStream = objWebResponse.GetResponseStream();

                        StreamReader readStream;

                        //Trường hợp đặc biệt, nguồn congan.com.vn
                        // || objWebResponse.ResponseUri.Host.Contains("vov.vn")

                        if (objWebResponse.ResponseUri.Host.Contains("sgtvt.hochiminhcity.gov.vn") || objWebResponse.ResponseUri.Host.Contains("congan.com.vn") )
                        {
                            readStream = new StreamReader(receiveStream, Encoding.GetEncoding("Windows-1252"));
                        }
                        else readStream = new StreamReader(receiveStream, Encoding.UTF8);
                     
                        
                        
                        
                        result = readStream.ReadToEnd();

                        if (result.Contains("<title>Redirect...</title>"))
                        {

                            int i = result.IndexOf("url=");
                            if (i == 0) i = result.IndexOf("URL=");
                            string url = result.Substring(i+4);
                            Match m1 = Regex.Match(url, @"\"">");
                            if (m1.Success)
                            {
                                url = url.Substring(0, m1.Index);
                            }

                            if (url.Contains("http://")) result = GetWebData(url);
                            else 
                            {
                                if (url.Length != 0)
                                {
                                    url = "http://" + objWebRequest.RequestUri.Host + url;
                                    result = GetWebData(url);
                                }
                            }

                        }

                        
                        readStream.Close();
                    }

                    //Move permanently

                    if ((int)objWebResponse.StatusCode == 301 || (int)objWebResponse.StatusCode == 307)
                    {
                        string uriString = objWebResponse.Headers["Location"];

                        if (!uriString.Contains("http://"))
                        {
                            uriString = "http://" + objWebResponse.ResponseUri.Host + uriString;
                        }

                        objWebResponse.Close();

                        result = GetWebData(uriString);
                    }
                   
                    objWebResponse.Close();
                }

                
                
                
            }
            catch(WebException e)
            {
            //    if (e.Status == WebExceptionStatus.ProtocolError)
            //    {
            //        WebResponse resp = e.Response;
            //        using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
            //        {
            //            System.Net.WebClient wc = new System.Net.WebClient();
            //            String s = wc.DownloadString(link);

            //            MessageBox.Show(sr.ReadToEnd());
            //        }
            //    }


                
                if (e.Message.Contains("(502) Bad Gateway"))
                {
                    if (link.Contains("dantri.com.vn"))
                    {
                        link = "https://www.google.com.vn/search?q=" + link + "&start=0&num=1";

                        string temp = GetWebData(link);

                        Match m = Regex.Match(temp , @"<h3 class=""r""><a href=""([^""]*)""");
                        Match url = Regex.Match(m.Value, @"(?i)\b((?:https?:(?:/{1,3}|[a-z0-9%])|[a-z0-9.\-]+[.](?:com|net|org|edu|gov|mil|aero|asia|biz|cat|coop|info|int|jobs|mobi|museum|name|post|pro|tel|travel|xxx|ac|ad|ae|af|ag|ai|al|am|an|ao|aq|ar|as|at|au|aw|ax|az|ba|bb|bd|be|bf|bg|bh|bi|bj|bm|bn|bo|br|bs|bt|bv|bw|by|bz|ca|cc|cd|cf|cg|ch|ci|ck|cl|cm|cn|co|cr|cs|cu|cv|cx|cy|cz|dd|de|dj|dk|dm|do|dz|ec|ee|eg|eh|er|es|et|eu|fi|fj|fk|fm|fo|fr|ga|gb|gd|ge|gf|gg|gh|gi|gl|gm|gn|gp|gq|gr|gs|gt|gu|gw|gy|hk|hm|hn|hr|ht|hu|id|ie|il|im|in|io|iq|ir|is|it|je|jm|jo|jp|ke|kg|kh|ki|km|kn|kp|kr|kw|ky|kz|la|lb|lc|li|lk|lr|ls|lt|lu|lv|ly|ma|mc|md|me|mg|mh|mk|ml|mm|mn|mo|mp|mq|mr|ms|mt|mu|mv|mw|mx|my|mz|na|nc|ne|nf|ng|ni|nl|no|np|nr|nu|nz|om|pa|pe|pf|pg|ph|pk|pl|pm|pn|pr|ps|pt|pw|py|qa|re|ro|rs|ru|rw|sa|sb|sc|sd|se|sg|sh|si|sj|Ja|sk|sl|sm|sn|so|sr|ss|st|su|sv|sx|sy|sz|tc|td|tf|tg|th|tj|tk|tl|tm|tn|to|tp|tr|tt|tv|tw|tz|ua|ug|uk|us|uy|uz|va|vc|ve|vg|vi|vn|vu|wf|ws|ye|yt|yu|za|zm|zw)/)(?:[^\s()<>{}\[\]]+|\([^\s()]*?\([^\s()]+\)[^\s()]*?\)|\([^\s]+?\))+(?:\([^\s()]*?\([^\s()]+\)[^\s()]*?\)|\([^\s]+?\)|[^\s`!()\[\]{};:'"".,<>?«»“”‘’])|(?:(?<!@)[a-z0-9]+(?:[.\-][a-z0-9]+)*[.](?:com|net|org|edu|gov|mil|aero|asia|biz|cat|coop|info|int|jobs|mobi|museum|name|post|pro|tel|travel|xxx|ac|ad|ae|af|ag|ai|al|am|an|ao|aq|ar|as|at|au|aw|ax|az|ba|bb|bd|be|bf|bg|bh|bi|bj|bm|bn|bo|br|bs|bt|bv|bw|by|bz|ca|cc|cd|cf|cg|ch|ci|ck|cl|cm|cn|co|cr|cs|cu|cv|cx|cy|cz|dd|de|dj|dk|dm|do|dz|ec|ee|eg|eh|er|es|et|eu|fi|fj|fk|fm|fo|fr|ga|gb|gd|ge|gf|gg|gh|gi|gl|gm|gn|gp|gq|gr|gs|gt|gu|gw|gy|hk|hm|hn|hr|ht|hu|id|ie|il|im|in|io|iq|ir|is|it|je|jm|jo|jp|ke|kg|kh|ki|km|kn|kp|kr|kw|ky|kz|la|lb|lc|li|lk|lr|ls|lt|lu|lv|ly|ma|mc|md|me|mg|mh|mk|ml|mm|mn|mo|mp|mq|mr|ms|mt|mu|mv|mw|mx|my|mz|na|nc|ne|nf|ng|ni|nl|no|np|nr|nu|nz|om|pa|pe|pf|pg|ph|pk|pl|pm|pn|pr|ps|pt|pw|py|qa|re|ro|rs|ru|rw|sa|sb|sc|sd|se|sg|sh|si|sj|Ja|sk|sl|sm|sn|so|sr|ss|st|su|sv|sx|sy|sz|tc|td|tf|tg|th|tj|tk|tl|tm|tn|to|tp|tr|tt|tv|tw|tz|ua|ug|uk|us|uy|uz|va|vc|ve|vg|vi|vn|vu|wf|ws|ye|yt|yu|za|zm|zw)\b/?(?!@)))");

                        if (m.Success)
                        {
                            link = url.Value;
                            result = GetWebData(link);
                        }
                        else result = "(502) Bad Gateway";
                        

                    }
                    else result = "(502) Bad Gateway";

                }
                
                
                
                

                return result;
            }

           //if (link.Contains("vov.vn"))
           //{
           //    result = WebUtility.HtmlDecode(result);
           //}

            return result;
        }

        public string ProcessArticle(string ArticleText)
        {
           
            #region Common
            ArticleText = Regex.Replace(ArticleText, @"\[\[[Cc]ategory", "[[Thể loại");
            ArticleText = Regex.Replace(ArticleText, @"\{\{[Cc]ite\s*[Bb]ook", "{{chú thích sách");
            ArticleText = Regex.Replace(ArticleText, @"\{\{[Cc]ite\s[Ww]eb", "{{chú thích web");
            ArticleText = Regex.Replace(ArticleText, @"\{\{[Rr]eflist", "{{tham khảo");
            ArticleText = Regex.Replace(ArticleText, @"\<[Rr]eferences\s*\/\>", "{{tham khảo}}");
            ArticleText = Regex.Replace(ArticleText, @"\{\{[Cc]ite\s*[Ee]pisode", "{{chú thích phần chương trình");
            ArticleText = ArticleText.Replace("\n ", "\n");
            ArticleText = Regex.Replace(ArticleText, @"\<\s*ref", "<ref");
            ArticleText = Regex.Replace(ArticleText, @"\<\s*\/\s*ref\s*>", "</ref>");
            ArticleText = Regex.Replace(ArticleText, @"\[\s*http", "[http");
            #endregion

            #region Punctuation
            // Punctuation
            ArticleText = Regex.Replace(ArticleText, @"\s\.", ".");
            ArticleText = Regex.Replace(ArticleText, @"\s\,", ",");
            ArticleText = Regex.Replace(ArticleText, @"\s\)", ")");
            ArticleText = Regex.Replace(ArticleText, @"\(\s", "(");
            ArticleText = Regex.Replace(ArticleText, @"\s\:", ":");
            ArticleText = Regex.Replace(ArticleText, @"\s\;", ";");
            #endregion

            #region Translation
            // Translate English -> Vietnamese
            ArticleText = Regex.Replace(ArticleText, @"==\s*[Ee]xternal\s*links\s*==", "== Liên kết ngoài ==");
            ArticleText = Regex.Replace(ArticleText, @"==\s*[Rr]eferences\s*==", "== Tham khảo ==");
            ArticleText = Regex.Replace(ArticleText, @"==\s*[Ss]ee\s*also\s*==", "== Xem thêm ==");
            ArticleText = Regex.Replace(ArticleText, @"==\s*[Ff]urther\s*reading\s*==", "== Đọc thêm ==");
            ArticleText = Regex.Replace(ArticleText, @"==\s*Notes\s*==", "== Ghi chú ==");
            #endregion

            return ArticleText;

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = "";
        }

        public void Execute()
        {
            //Test a = new Test();
            //a.Show();

            string originContent = ProcessArticle(this.textBox1.Text);
            this.textBox2.Text = "";

            progressBar1.Minimum = 0;
            progressBar1.Value = 1;
            progressBar1.Step = 1;

            if (originContent.Length != 0)
            {

                GetRefs(originContent);
                progressBar1.Maximum = refs.Count * 2 + 1;

                if (refs.Count != 0)
                {

                    ConvertText();
                    FormatRepeatRefs();

                    for (int i = 0; i < refs.Count; i++)
                    {
                        originContent = ReplaceFirst(originContent, refs[i], convertedrefs[i]);
                        progressBar1.PerformStep();
                    }



                }



                this.textBox2.Text = originContent;

               // string wiki = String.Empty;
               // wiki = comboBox2.SelectedItem.ToString();
               // wiki = wiki.Substring(0, 2);

               // Uri u = new Uri("https://" + wiki + ".wikipedia.org/w/index.php?title=" + this.textBox3.Text + "&action=edit");

               //// this.webBrowser1.Document.Forms["editform"].InnerText = originContent;

               // this.webBrowser1.Url = u;
                





                progressBar1.PerformStep();


            }
            else
            {
                progressBar1.Maximum = 1;
                progressBar1.PerformStep();
            }


            while (progressBar1.Value != progressBar1.Maximum)
            {
                progressBar1.PerformStep();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Execute();
        }

        private void Converter_Load(object sender, EventArgs e)
        {

            this.KeyPreview = true;

            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            this.textBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox2_KeyDown);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode.ToString() == "A")
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                this.KeyPreview = true;
                this.textBox1.SelectAll();
            }
        }

        private void Converter_KeyDown(object sender, KeyEventArgs e)
        {

            //if (e.Control && e.KeyCode.ToString() == "A")
            //{

            //    this.KeyPreview = true;

            //}

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode.ToString() == "A")
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                this.KeyPreview = true;
                this.textBox2.SelectAll();
            }
        }

        private void authorToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("My name is Alphama. I am from Vietnamese Wikipedia. My email is \"alphamawikipedia@gmail.com\". Do you want to contact me?", "Author", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("https://vi.wikipedia.org/wiki/User:Alphama");
            }
        }

        private void versionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Alphama Converter 1.0 \r\nThis tool is used for converting words and terms among different Wikipedias. ", "Version");
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This tool is used for translating terms, templates and categories among Wikipedia language versions. \r\n\r\n 1. Copy & Paste content to left textbox \r\n 2. Choose origin and converted languages \r\n 3. Click Convert button and receive result in right textbox.", "Help");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.Clear();


                Clipboard.SetText(this.textBox2.Text);
            }
            catch
            {
                Clipboard.SetText(" ");
            }

        }

        public string StartProcess(string fileName, string args, bool hideWindow)
        {
            string result = "";
            //Create a new Process object
            Process p = new Process();

            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;

            //Set the file that will be executed
            p.StartInfo.FileName = fileName;

            //Set the parameters to start the process with
            p.StartInfo.Arguments = args;

            //Hide the window (if applicable), or display normally
            p.StartInfo.WindowStyle = (hideWindow ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal);

            //Start the process
            p.Start();

            result = p.StandardOutput.ReadToEnd();

            //Block this thread until the process completes
            p.WaitForExit();

            return result;
        }

        public static string GetWikiData(string link)
        {
            string result = String.Empty;

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            ServicePointManager.DefaultConnectionLimit = 9999;
            //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12

            WebRequest objWebRequest = WebRequest.Create(link);
            objWebRequest.Credentials = CredentialCache.DefaultCredentials;
            ((HttpWebRequest)objWebRequest).UserAgent = ".NET Framework Example Client";
            WebResponse objWebResponse = objWebRequest.GetResponse();
            Stream receiveStream = objWebResponse.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, System.Text.Encoding.UTF8);
            result = readStream.ReadToEnd();
            objWebResponse.Close();
            readStream.Close();
            return result;
        }

        // Kiểm tra với tất cả ngôn ngữ có liên kết ngôn ngữ với bài viết tiếng Việt hay không?
        public string GetArticlebyContent(string title)
        {
            string strContent = String.Empty;

            string wiki = String.Empty;
            wiki = comboBox2.SelectedItem.ToString();
            wiki = wiki.Substring(0, 2);

          

            string link = "http://" + wiki + ".wikipedia.org/w/api.php?format=xml&action=query&prop=revisions&rvprop=content&titles=" + title;

            try
            {
                strContent = GetWikiData(link);
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(strContent);
                XmlNodeList pages = xd.SelectNodes("/api/query/pages/page/revisions/rev");


                if (pages.Count == 1)
                {
                    strContent = pages[0].InnerText;
                }

            }
            catch
            {
                return strContent;
            }


            strContent = strContent.Replace("\n", "\r\n");

            return strContent;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = GetArticlebyContent(this.textBox3.Text);
        }

        private void convertAllRefsExceptchúThíchTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (options1.Checked == false)
            {
                options1.CheckState = CheckState.Checked;
            }
            else
            {
                options1.CheckState = CheckState.Unchecked;
                
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        public string ProcessArticle(string ArticleText, string ArticleTitle, int wikiNamespace, out string Summary, out bool Skip)
        {
            Skip = false;
            Summary = "General fixes";
            string originVersion = ArticleText;

            // Choose main & category namespaces only
            if (wikiNamespace != 0 && wikiNamespace != 14)
            {
                Skip = true;
                return ArticleText;
            }

            Match m = Regex.Match(ArticleText, @"==Tham khảo==");
            Match m1 = Regex.Match(ArticleText, @"</references>");
            Match m2 = Regex.Match(ArticleText, @"==Liên kết ngoài==");
            Match m3 = Regex.Match(ArticleText, @"\{\{[Ss]ơ\s*[Kk]hai");

            // Insert right position
            if (m.Success && m1.Success && m2.Success && m1.Index > m.Index && m.Index > m2.Index)
            {
              
                ArticleText = ArticleText.Insert(m2.Index, ArticleText.Substring(m.Index, m1.Index - m.Index)+"\r\n");
            }
            else
            {
                if (m3.Success && m.Success && m1.Success && m1.Index > m.Index && m.Index > m3.Index)
                {
                    ArticleText = ArticleText.Insert(m3.Index, ArticleText.Substring(m.Index, m1.Index - m.Index));
                }
            }

            if (originVersion != ArticleText)
            {

                // Delete abudant part
                int i = ArticleText.LastIndexOf("==Tham khảo==");
                int j = ArticleText.LastIndexOf("</references>");

                string tempj = "</references>";

                if (j > i)
                {
                    string part1 = ArticleText.Substring(0, i - 1);
                    string part2 = ArticleText.Substring(j + tempj.Length + 1, ArticleText.Length - j - tempj.Length - 1);
                    ArticleText = part1 + part2;
                }
            }

            if (originVersion == ArticleText) Skip = true;
            return ArticleText;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AddBareLink testDialog = new AddBareLink();
            testDialog.ShowDialog();
            this.textBox1.Text += testDialog.text;
        }

        private void convertAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (options2.Checked == false)
            {
                options2.CheckState = CheckState.Checked;
            }
            else
            {
                options2.CheckState = CheckState.Unchecked;

            }
        }

    }
}
