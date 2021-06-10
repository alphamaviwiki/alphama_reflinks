using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlphamaConverter
{
	public class URLInfo
	{

        public List<URLInfo> list;

        string urlLink;
        public string UrlLink
        {
            get { return urlLink; }
            set { urlLink = value; }
        }

        string siteName;
        public string SiteName
        {
            get { return siteName; }
            set { siteName = value; }
        }

        string urlTitle;
        public string UrlTitle
        {
            get { return urlTitle; }
            set { urlTitle = value; }
        }

        string datePublished;
        public string DatePublished
        {
            get { return datePublished; }
            set { datePublished = value; }
        }

        string dateRetrieved;
        public string DateRetrieved
        {
            get { return dateRetrieved; }
            set { dateRetrieved = value; }
        }

        string language;
        public string Language
        {
            get { return language; }
            set { language = value; }
        }

        string author;
        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        public URLInfo()
        {

        }

        public URLInfo(string urlLink, string siteName, string language)
        {
            this.UrlLink = urlLink;
            this.SiteName = siteName;
            this.Language = language;
        }

        public void LoadURL()
        {
            list = new List<URLInfo>();

            list.Add(new URLInfo("2sao.vn", "Trang thông tin điện tử tổng hợp của Công ty Cổ phần Truyền thông VietNamNet", "Tiếng Việt"));
            list.Add(new URLInfo("afamily.vn", "Afamily.vn", "Tiếng Việt"));
            list.Add(new URLInfo("anninhthudo.vn", "Báo An ninh Thủ đô", "Tiếng Việt"));
            list.Add(new URLInfo("baobacninh.com.vn", "[[Báo Bắc Ninh]]", "Tiếng Việt"));
            list.Add(new URLInfo("baobinhdinh.com.vn", "Báo Bình Định", "Tiếng Việt"));
            list.Add(new URLInfo("baocamau.com.vn", "Báo Cà Mau", "Tiếng Việt"));
            list.Add(new URLInfo("baodatviet.vn", "Báo Đất Việt", "Tiếng Việt"));
            list.Add(new URLInfo("baodientu.chinhphu.vn", "Báo Điện tử Chính Phủ nước CHXNCN Việt Nam", "Tiếng Việt"));
            list.Add(new URLInfo("baohatinh.vn", "Hà Tĩnh Online", "Tiếng Việt"));
            list.Add(new URLInfo("baoninhbinh.org.vn", "Báo Ninh Bình", "Tiếng Việt"));
            list.Add(new URLInfo("baoninhthuan.com.vn", "Báo Ninh Thuận", "Tiếng Việt"));
            list.Add(new URLInfo("baoquangngai.vn", "Báo Điện tử Quảng Ngãi", "Tiếng Việt"));
            list.Add(new URLInfo("baoquangtri.vn", "Báo Quảng Trị", "Tiếng Việt"));
            list.Add(new URLInfo("baothaibinh.com.vn", "Báo Thái Bình", "Tiếng Việt"));
            list.Add(new URLInfo("baothainguyen.vn", "[[Thái Nguyên (báo)|Báo Thái Nguyên]]", "Tiếng Việt"));
            list.Add(new URLInfo("baothuathienhue.vn", "Báo Thừa Thiên Huế", "Tiếng Việt"));
            list.Add(new URLInfo("baothuongmai.com.vn", "[[Báo Thương mại (Việt Nam)|Báo Thương Mại]]", "Tiếng Việt"));
            list.Add(new URLInfo("baotintuc.vn", "Báo Tin tức - Kênh thông tin CP do TTXVN phát hành", "Tiếng Việt"));
            list.Add(new URLInfo("baoyenbai.com.vn", "Báo Yên Bái", "Tiếng Việt"));
            list.Add(new URLInfo("bbc.com", "BBC News", "Tiếng Anh"));
            list.Add(new URLInfo("binhdinh.gov.vn", "Cổng TTĐT tỉnh Bình Định", "Tiếng Việt"));
            list.Add(new URLInfo("bongdaplus.vn", "[[Báo Bóng đá]]", "Tiếng Việt"));
            list.Add(new URLInfo("bongdaso.com", "Bóng đá số", "Tiếng Việt"));
            list.Add(new URLInfo("cand.com.vn", "Báo Công an Nhân dân Điện tử", "Tiếng Việt"));
            list.Add(new URLInfo("chinhphu.vn", "Cổng Thông tin Điện tử Chính phủ", "Tiếng Việt"));
            list.Add(new URLInfo("citinews.net", "CitiNews", "Tiếng Việt"));
            list.Add(new URLInfo("ckt.gov.vn", "Cục Kinh tế - Bộ Quốc phòng", "Tiếng Việt"));
            list.Add(new URLInfo("congan.com.vn", "Báo Công an Thành Phố Hồ Chí Minh", "Tiếng Việt"));
            list.Add(new URLInfo("daidoanket.vn", "[[Báo Đại Đoàn Kết]]", "Tiếng Việt"));
            list.Add(new URLInfo("dangcongsan.vn", "[[Tạp chí Cộng sản|Báo điện tử Đảng Cộng sản Việt Nam]]", "Tiếng Việt"));
            list.Add(new URLInfo("dantri.com.vn", "[[Dân trí (báo)|Báo điện tử Dân Trí]]", "Tiếng Việt"));
            list.Add(new URLInfo("danviet.vn", "Báo điện tử báo Nông thôn Ngày nay", "Tiếng Việt"));
            list.Add(new URLInfo("dddn.com.vn", "Báo điện tử Diễn đàn Doanh Nghiệp", "Tiếng Việt"));
            list.Add(new URLInfo("dinhduong.com.vn", "Chuyên trang Dinh dưỡng & Sức khỏe", "Tiếng Việt"));
            list.Add(new URLInfo("doisongphapluat.com", "Báo đời sống & pháp luật Online", "Tiếng Việt"));
            list.Add(new URLInfo("eva.vn", "Eva.vn", "Tiếng Việt"));
            list.Add(new URLInfo("giadinh.net.vn", "Báo điện tử của Báo Gia đình và Xã hội", "Tiếng Việt"));
            list.Add(new URLInfo("giaoducthoidai.vn", "Báo Giáo dục và Thời đại", "Tiếng Việt"));
            list.Add(new URLInfo("news.go.vn", "Go.vn", "Tiếng Việt"));
            list.Add(new URLInfo("hanoimoi.com.vn", "[[Hànộimới|Báo Hànộimới]]", "Tiếng Việt"));
            list.Add(new URLInfo("hoahoctro.vn", "[[Hoa Học Trò (báo)|Hoa Học Trò]]", "Tiếng Việt"));
            list.Add(new URLInfo("hoinongdan.org.vn", "Hội Nông dân Việt Nam", "Tiếng Việt"));
            list.Add(new URLInfo("hoinongdanqnam.org.vn", "Hội Nông dân Quảng Nam", "Tiếng Việt"));
            list.Add(new URLInfo("ibongda.vn", "ibongda.vn", "Tiếng Việt"));
            list.Add(new URLInfo("kcmdanang.org.vn", "Trung tâm Thông tin Khoa học và Công nghệ Đà Nẵng", "Tiếng Việt"));
            list.Add(new URLInfo("khoahocthoidai.vn", "Khoa học Thời đại Online", "Tiếng Việt"));
            list.Add(new URLInfo("khuyennongvn.gov.vn", "Trung tâm Khuyến nông Quốc gia", "Tiếng Việt"));
            list.Add(new URLInfo("lamdong.gov.vn", "Cổng thông tin điện tử Lâm Đồng", "Tiếng Việt"));
            list.Add(new URLInfo("langson.gov.vn", "Cổng TTĐT tỉnh Lạng Sơn", "Tiếng Việt"));
            list.Add(new URLInfo("laocai.gov.vn", "Cổng TTĐT tỉnh Lào Cai", "Tiếng Việt"));
            list.Add(new URLInfo("laodong.com.vn", "[[Lao Động (báo)|Báo Lao Động]]", "Tiếng Việt"));
            list.Add(new URLInfo("ngoisao.net", "Chuyên mục văn hoá giải trí của VnExpress", "Tiếng Việt"));
            list.Add(new URLInfo("nguoiduatin.vn", "Báo điện tử Người đưa tin", "Tiếng Việt"));
            list.Add(new URLInfo("nguoi-viet.com", "Người Việt", "Tiếng Việt"));
            list.Add(new URLInfo("nhandan.com.vn", "[[Nhân Dân (báo)|Báo điện tử Nhân Dân]]", "Tiếng Việt"));
            list.Add(new URLInfo("nhandaovadoisong.com.vn", "Nhân Đạo & Đời Sống", "Tiếng Việt"));
            list.Add(new URLInfo("nld.com.vn", "[[Người lao động (báo)|Người Lao Động]]", "Tiếng Việt"));
            list.Add(new URLInfo("nongnghiep.vn", "Báo Nông nghiệp Việt Nam", "Tiếng Việt"));
            list.Add(new URLInfo("phununet.com", "PhunuNet", "Tiếng Việt"));
            list.Add(new URLInfo("phunuonline.com.vn", "Báo Phụ Nữ Thành Phố Hồ Chí Minh", "Tiếng Việt"));
            list.Add(new URLInfo("plo.vn", "Báo điện tử Pháp Luật thành phố Hồ Chí Minh", "Tiếng Việt"));
            list.Add(new URLInfo("qdnd.vn", "Quân đội Nhân dân", "Tiếng Việt"));
            list.Add(new URLInfo("quangtri.gov.vn", "Cổng TTĐT tỉnh Quảng Trị", "Tiếng Việt"));
            list.Add(new URLInfo("rfa.org", "[[Đài Á Châu Tự do]]", "Tiếng Việt"));
            list.Add(new URLInfo("sggp.org.vn", "[[Sài Gòn Giải Phóng|Báo Sài Gòn Giải Phóng Online]]", "Tiếng Việt"));
            list.Add(new URLInfo("sonla.gov.vn", "Cổng thông tin điện tử tỉnh Sơn La", "Tiếng Việt"));
            list.Add(new URLInfo("suckhoedoisong.vn", "Báo Sức khỏe & Đời sống", "Tiếng Việt"));
            list.Add(new URLInfo("tapchilamdep.com", "Tạp chí Làm đẹp", "Tiếng Việt"));
            list.Add(new URLInfo("thaibinh.gov.vn", "Cổng thông tin điện tử Thái Bình", "Tiếng Việt"));
            list.Add(new URLInfo("thanhnien.com.vn", "[[Thanh Niên (báo)|Thanh Niên Online]]", "Tiếng Việt"));
            list.Add(new URLInfo("thegioivanhoa.com.vn", "[[Thế Giới Văn Hóa (tạp chí)|Tạp chí Thế giới Văn hóa]]", "Tiếng Việt"));
            list.Add(new URLInfo("thethaovanhoa.vn", "Báo Thể thao & Văn hóa - Thông tấn xã Việt Nam", "Tiếng Việt"));
            list.Add(new URLInfo("thongtinkhcn.com.vn", "Thông tin Khoa học Công nghệ Bắc Giang", "Tiếng Việt"));
            list.Add(new URLInfo("thucphamvadoisong.vn", "Thực phẩm & Đời sống", "Tiếng Việt"));
            list.Add(new URLInfo("tiepthigiadinh.com.vn", "[[Tiếp thị & Gia đình]]", "Tiếng Việt"));
            list.Add(new URLInfo("tienphong.vn", "Báo Điện tử Tiền Phong", "Tiếng Việt"));
            list.Add(new URLInfo("timnhanh.com", "Tìm Nhanh", "Tiếng Việt"));
            list.Add(new URLInfo("tin247.com", "Tin 247", "Tiếng Việt"));
            list.Add(new URLInfo("tinmoi.vn", "Tin Mới Online", "Tiếng Việt"));
            list.Add(new URLInfo("tintuconline.com.vn", "Tin Tức Online", "Tiếng Việt"));
            list.Add(new URLInfo("tuoitre.vn", "[[Tuổi Trẻ (báo)|Tuổi Trẻ Online]]", "Tiếng Việt"));
            list.Add(new URLInfo("vcn.vnn.vn", "Viện chăn nuôi - Bộ Nông nghiệp & Phát triển Nông thôn", "Tiếng Việt"));
            list.Add(new URLInfo("viet.rfi.fr", "[[Radio France Internationale|RFI Tiếng Việt]]", "Tiếng Việt"));
            list.Add(new URLInfo("vietbao.vn", "[[Vietbao|Việt Báo]]", "Tiếng Việt"));
            list.Add(new URLInfo("vietlinh.vn", "Trang tin điện tử Việt Linh", "Tiếng Việt"));
            list.Add(new URLInfo("vietnamnet.vn", "[[VietNamNet]]", "Tiếng Việt"));
            list.Add(new URLInfo("vietnamplus.vn", "[[Thông tấn xã Việt Nam]]", "Tiếng Việt"));
            list.Add(new URLInfo("vinhlong.gov.vn", "Cổng TTĐT UBND tỉnh Vĩnh Long", "Tiếng Việt"));
            list.Add(new URLInfo("vinhphuc.gov.vn", "Cổng TTĐT tỉnh Vĩnh Phúc", "Tiếng Việt"));
            list.Add(new URLInfo("vir.com.vn", "[[Vietnam Investment Review]]", "Tiếng Anh"));
            list.Add(new URLInfo("vneconomy.vn", "VnEconomy, báo điện tử thuộc nhóm Thời báo Kinh tế Việt Nam", "Tiếng Việt"));
            list.Add(new URLInfo("vnexpress.net", "[[VnExpress|VnExpress - Tin nhanh Việt Nam]]", "Tiếng Việt"));
            list.Add(new URLInfo("vnmedia.vn", "[[VnMedia|Báo điện tử VnMedia - Tập đoàn Bưu chính Viễn thông Việt Nam]]", "Tiếng Việt"));
            list.Add(new URLInfo("vov.vn", "[[Đài Tiếng nói Việt Nam|Báo Điện tử Đài Tiếng nói Việt Nam]]", "Tiếng Việt"));
            list.Add(new URLInfo("vtc.vn", "Báo điện tử VTC News", "Tiếng Việt"));
            list.Add(new URLInfo("vtv.vn", "Báo điện tử của Đài Truyền hình Việt Nam", "Tiếng Việt"));
            list.Add(new URLInfo("zing.vn", "Zing.vn", "Tiếng Việt"));
            list.Add(new URLInfo("24h.com.vn", "24h.com.vn", "Tiếng Việt"));
            list.Add(new URLInfo("baohoabinh.com.vn", "Báo Hòa Bình", "Tiếng Việt"));
            list.Add(new URLInfo("giacngo.vn", "Giác Ngộ Online", "Tiếng Việt"));
            list.Add(new URLInfo("kiengiang.gov.vn", "UBND tỉnh Kiên Giang", "Tiếng Việt"));
            list.Add(new URLInfo("baopnvn.com", "[[Báo Đại Đoàn Kết]]", "Tiếng Việt"));
            list.Add(new URLInfo("24h.com.vn", "24h.com.vn", "Tiếng Việt"));
            list.Add(new URLInfo("asc-aqua.org", "Aquaculture Stewardship Council (ASC)", "Tiếng Anh"));
            list.Add(new URLInfo("diaoconline.vn", "Địa Ốc Online", "Tiếng Việt"));
            list.Add(new URLInfo("tinthethao.com.vn", "Tin Thể Thao", "Tiếng Việt"));
            list.Add(new URLInfo("hochiminhcity.gov.vn", "Mạng Thông Tin tích hợp trên Internet của TP HCM", "Tiếng Việt"));
            list.Add(new URLInfo("baoanhdatmui.vn", "Báo ảnh Đất Mũi Online ", "Tiếng Việt"));

            list.Add(new URLInfo("baoanhdatmui.vn", "Báo ảnh Đất Mũi Online ", "Tiếng Việt"));
            list.Add(new URLInfo("baoanhdatmui.vn", "Báo ảnh Đất Mũi Online ", "Tiếng Việt"));
            list.Add(new URLInfo("baoanhdatmui.vn", "Báo ảnh Đất Mũi Online ", "Tiếng Việt"));
            



        }

        public URLInfo SearchbyURLink(string url)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (url.Contains(list[i].UrlLink))
                {
                    return list[i];
                }
            }
            return null;
        }

        


	}
}
