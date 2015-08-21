using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using n11Api.ServiceReference1;
using System.Net;

using System.ServiceModel;
//using Microsoft.Web.Services3.Messaging;
using n11Api.com.n11.api;

namespace n11Api
{
    public partial class Form1 : Form
    {
        static void setProxy()
        {

            //IWebProxy aProxy = WebRequest.DefaultWebProxy;

            WebProxy proxy = (WebProxy)WebProxy.GetDefaultProxy();
            if (proxy.Address != null)
            {
                proxy.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                WebRequest.DefaultWebProxy = new System.Net.WebProxy(proxy.Address, proxy.BypassProxyOnLocal, proxy.BypassList, proxy.Credentials);
            }
        }




        static string apiAnahtari1;
        static string apiSifresi1;


        public Form1()
        {
            InitializeComponent();

        }

        public Form1(string apiAnahtari, string apiSifresi)
        {
            InitializeComponent();
            apiAnahtari1 = apiAnahtari;
            apiSifresi1 = apiSifresi;
        }
        
        ProductBasic[] products = getProds();

       





        private void Form1_Load(object sender, EventArgs e)
        {
 GetTopLevelCategoriesResponse categories = getTopCat();

            foreach (var item in categories.categoryList)
            {


                lstBxAnaKategori.DisplayMember = "name";
                lstBxAnaKategori.ValueMember = "id";

                lstBxAnaKategori.Items.Add(new Category() { id = item.id, name = item.id + "-" + item.name });

            }

            getProds();

            foreach (var prod in products)
            {

                listBox2.DisplayMember = "title";
                listBox2.ValueMember = "id";
                listBox2.Items.Add(new ProductBasic() { id = prod.id, title = prod.id + "-(" + prod.productSellerCode.ToString() + ")-" + prod.title });
            }

            txtprodPrepDay.Text = "21";
            txtUrunDurumu.Text = "1";
            txtKargoSablonu.Text = "dizlik";
            txtStokMiktari.Text = "1";
            label2.Text = "Ürünler (" + products.Length.ToString() + ")";

        }

        private static GetTopLevelCategoriesResponse getTopCat()
        {
            var authentication = new n11Api.ServiceReference1.Authentication();
            
            authentication.appKey = apiAnahtari1;  //api anahtarınız

            authentication.appSecret = apiSifresi1;//api şifeniz

            CategoryServicePortClient proxy = new CategoryServicePortClient();

            var request = new GetTopLevelCategoriesRequest();
            request.auth = authentication;


            var categories = proxy.GetTopLevelCategories(request);

            return categories;
        }

        private static ProductBasic[] getProds()
        {
            setProxy();

            var authentication1 = new com.n11.api.Authentication();
            authentication1.appKey = "0269671d-74f9-492e-8ab4-8d3825c48715"; //api anahtarınız
            authentication1.appSecret = "VOZHd5LSaHXDdJXZ";//api şifeniz


            com.n11.api.GetProductListRequest ProductListRequest = new com.n11.api.GetProductListRequest();
            ProductListRequest.auth = authentication1;

            com.n11.api.ProductServicePortService port = new ProductServicePortService();



            com.n11.api.GetProductListResponse response = port.GetProductList(ProductListRequest);
            var products = response.products;
            return products;
        }

        private static Product product(long prodId)
        {
            var authentication1 = new com.n11.api.Authentication();
            authentication1.appKey = apiAnahtari1; //api anahtarınız
            authentication1.appSecret = apiSifresi1;//api şifeniz

            ProductServicePortService proxy = new ProductServicePortService();
            GetProductByProductIdRequest request = new GetProductByProductIdRequest();
            request.auth = authentication1;
            request.productId = prodId;

            GetProductByProductIdResponse response = new GetProductByProductIdResponse();
            var product = proxy.GetProductByProductId(request).product;

            return product;

        }

        private string retSaleStatus(int i)
        {
            string SaleStatus;

            if (i == 1)
                SaleStatus = "Satış Öncesi";
            else if (i == 2)
                SaleStatus = "Satışta";
            else if (i == 3)
                SaleStatus = "Stok yok";
            else
                SaleStatus = "Satışa Kapalı";
            return SaleStatus;
        }

        private string retAppStatus(int i)
        {
            string AppStatus;

            if (i == 1)
                AppStatus = "Aktif";
            else if (i == 2)
                AppStatus = "Beklemede";
            else
                AppStatus = "Yasaklı";
            return AppStatus;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            foreach (Control txt in this.Controls)
                if (txt is TextBox || txt is ComboBox || txt is RichTextBox)
                    txt.Text = "";
            urunResimlericmbBox.Items.Clear();
            urunResimlericmbBox.SelectedIndex = -1;
            urunResimlericmbBox.Text = "";
            var lstItem = listBox2.SelectedItem as ProductBasic;

            ProductBasic[] products = getProds();


            foreach (var produc in products)
                if (produc.id == lstItem.id)
                {

                    var prod = product(produc.id);
                    Product produ = getProdDetails(produc.id);

                    txtMagazaKodu.Text = produ.productSellerCode;
                    txtUrunBasligi.Text = produ.title;
                    richTextBox1.Text = produ.description;
                    txtaltBaslik.Text = produ.subtitle;
                    richTextBox1.Text = produ.description;
                    txtUrunKategoriNo.Text = produ.category.id.ToString();
                    txtFiyati.Text = produ.price.ToString();
                    for (int i = 0; i < produ.images.Length; i++)
                    { urunResimlericmbBox.Items.Add(produ.images[i].url); }
                    txtUrunDurumu.Text = produ.productCondition;
                    txtprodPrepDay.Text = produ.preparingDay;
                    txtKargoSablonu.Text = produ.shipmentTemplate;
                    txtGoruntulelnFita.Text = prod.displayPrice.ToString();
                    txtStokMiktari.Text = produ.stockItems.stockItem.ToString();

                    //var stokOpsList = produc.stockItems.stockItem;
                    //if (stokOpsList == null)
                    //{ }
                    //else {
                    //    for (int i = 0; i < stokOpsList.Length; i++)
                    //        cmbBoxStokOpsiyonlariListesi.Items.Add(stokOpsList[i].attributes[0].name + "-" + stokOpsList[i].attributes[0].value);


                    txtFullKategori.Text = prod.category.fullName;
                    txtFiyati.Text = prod.price.ToString();
                    txtSatisStatusu.Text = retSaleStatus(Convert.ToInt32(produc.saleStatus));
                    txtOnayDurumu.Text = retAppStatus(Convert.ToInt32(produc.approvalStatus));

                }

        }

        private static Product getProdDetails(long prodId)
        {
            var authentication1 = new com.n11.api.Authentication();
            authentication1.appKey = "0269671d-74f9-492e-8ab4-8d3825c48715"; //api anahtarınız
            authentication1.appSecret = "VOZHd5LSaHXDdJXZ";//api şifeniz


            com.n11.api.GetProductByProductIdRequest ProductRequest = new com.n11.api.GetProductByProductIdRequest();
            ProductRequest.auth = authentication1;

            com.n11.api.ProductServicePortService port = new ProductServicePortService();

            ProductRequest.productId = prodId;

            com.n11.api.GetProductByProductIdResponse response = port.GetProductByProductId(ProductRequest);
            var product = response.product;
            return product;
        }

        private static SubCategoryData[] getSubCat(long CatId)
        {

            var authentication1 = new n11Api.ServiceReference1.Authentication();
            authentication1.appKey = "0269671d-74f9-492e-8ab4-8d3825c48715"; //api anahtarınız
            authentication1.appSecret = "VOZHd5LSaHXDdJXZ";//api şifeniz


            CategoryServicePortClient port = new CategoryServicePortClient();

            GetSubCategoriesRequest getSubbCatt = new GetSubCategoriesRequest();
            getSubbCatt.categoryId = CatId;
            getSubbCatt.auth = authentication1;
            GetSubCategoriesResponse response = port.GetSubCategories(getSubbCatt);
            var subCategory = response.category;
            return subCategory;
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstBxikinciKategori.Items.Clear();
            lstBxUcuncuKategori.Items.Clear();
            lstBxikinciKategori.Enabled = true;
            lstBxUcuncuKategori.Enabled = true;
            lstBxBirinciKategori.Enabled = true;
            if (lstBxBirinciKategori.SelectedItem == null)
            {
            }
            else
            {
                var CatId = lstBxBirinciKategori.SelectedItem.ToString().Substring(0, 7);
                SubCategoryData subData = getSubCat(Convert.ToInt32(CatId)).First();
                seciliKategori = subData.id.ToString();

                if (subData.subCategoryList == null)
                {
                    lstBxikinciKategori.Items.Add("Alt kategori bulunamadı");
                    lstBxikinciKategori.Enabled = false;
                }
                else
                {
                    for (int i = 0; i < subData.subCategoryList.Length; i++)
                        lstBxikinciKategori.Items.Add(subData.subCategoryList[i].id + "-" + subData.subCategoryList[i].name);

                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstBxBirinciKategori.Items.Clear();
            lstBxikinciKategori.Items.Clear();
            lstBxUcuncuKategori.Items.Clear();
            lstBxikinciKategori.Enabled = true;
            lstBxUcuncuKategori.Enabled = true;
            lstBxBirinciKategori.Enabled = true;

            var CatId = lstBxAnaKategori.SelectedItem as Category;
            SubCategoryData subData = getSubCat(CatId.id).First();
            seciliKategori = subData.id.ToString();


            if (subData.subCategoryList == null)
            {
                lstBxBirinciKategori.Items.Add("Alt kategori bulunamadı");
                lstBxBirinciKategori.Enabled = false;
            }
            else
            {
                for (int i = 0; i < subData.subCategoryList.Length; i++)
                    lstBxBirinciKategori.Items.Add(subData.subCategoryList[i].id + "-" + subData.subCategoryList[i].name);

            }
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstBxUcuncuKategori.Items.Clear();
            lstBxikinciKategori.Enabled = true;
            lstBxUcuncuKategori.Enabled = true;
            lstBxBirinciKategori.Enabled = true;

            if (lstBxikinciKategori.SelectedItem == null)
            {
            }
            else
            {
                var CatId = lstBxikinciKategori.SelectedItem.ToString().Substring(0, 7);
                SubCategoryData subData = getSubCat(Convert.ToInt32(CatId)).First();
                seciliKategori = subData.id.ToString();

                if (subData.subCategoryList == null)
                {
                    lstBxUcuncuKategori.Items.Add("Alt kategori bulunamadı");
                    lstBxUcuncuKategori.Enabled = false;
                }
                else
                {

                    for (int i = 0; i < subData.subCategoryList.Length; i++)
                        lstBxUcuncuKategori.Items.Add(subData.subCategoryList[i].id.ToString() + "-" + subData.subCategoryList[i].name);

                }
            }
        }

        private void prodPrepDayLxl_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var authentication1 = new com.n11.api.Authentication();
            authentication1.appKey = "0269671d-74f9-492e-8ab4-8d3825c48715"; //api anahtarınız
            authentication1.appSecret = "VOZHd5LSaHXDdJXZ";//api şifeniz

            ProductServicePortService prodServ = new ProductServicePortService();

            SaveProductRequest saveRequest = new SaveProductRequest();
            //marka, tarih aralığı
            saveRequest.auth = authentication1;
            saveRequest.product = new ProductRequest();
            saveRequest.product.productSellerCode = txtMagazaKodu.Text;

            saveRequest.product.title = txtUrunBasligi.Text;
            saveRequest.product.subtitle = txtaltBaslik.Text;
            saveRequest.product.description = richTextBox1.Text;
            saveRequest.product.category = new CategoryRequest();
            saveRequest.product.category.id = Convert.ToInt32(txtUrunKategoriNo.Text);
            saveRequest.product.price = Convert.ToDecimal(txtFiyati.Text);
            saveRequest.product.currencyType = comboBox1.Text;

            ProductImage prImg = new ProductImage();
            ProductImage[] pr = new ProductImage[1];
            pr[0] = prImg;
            string resimUrl = urunResimlericmbBox.Text.Replace("https", "http");
            prImg.url = resimUrl;
            prImg.order = "1";
            saveRequest.product.images = pr;

            saveRequest.product.productCondition = txtUrunDurumu.Text;
            saveRequest.product.preparingDay = txtprodPrepDay.Text;
            saveRequest.product.shipmentTemplate = txtKargoSablonu.Text;

            ProductSkuRequest prStock = new ProductSkuRequest();
            prStock.quantity = txtStokMiktari.Text;
            prStock.optionPrice = Convert.ToDecimal(txtFiyati.Text);
            ProductSkuRequest[] prStockList = new ProductSkuRequest[1];
            prStockList[0] = prStock;

            saveRequest.product.stockItems = prStockList;

            SaveProductResponse saveResponse = prodServ.SaveProduct(saveRequest);
            if (saveResponse.result.errorCode == null)
            {
                MessageBox.Show("ürün kaydedildi");
            }
            else
            {
                MessageBox.Show("ürün kaydedilmedi");
                MessageBox.Show(saveResponse.result.errorMessage);
            }

        }

        private static ParentCategoryData getParentCat(long CatId)
        {

            var authentication = new n11Api.ServiceReference1.Authentication();
            authentication.appKey = "0269671d-74f9-492e-8ab4-8d3825c48715"; //api anahtarınız
            authentication.appSecret = "VOZHd5LSaHXDdJXZ";//api şifeniz

            CategoryServicePortClient proxy = new CategoryServicePortClient();
            GetParentCategoryRequest request = new GetParentCategoryRequest();
            request.auth = authentication;
            request.categoryId = CatId;
            GetParentCategoryResponse parentCat = proxy.GetParentCategory(request);
            ParentCategoryData parentCatData = parentCat.category;
            return parentCat.category;

        }
        ParentCategoryData parentCat = new ParentCategoryData();
        private void button2_Click(object sender, EventArgs e)
        {
            string catdatid = lstBxAnaKategori.SelectedItem.ToString().Substring(0, 7);
            parentCat = getParentCat(Convert.ToInt32(catdatid));

            MessageBox.Show(parentCat.name);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            listBox2.Items.Clear();
            ProductBasic[] products = getProds();

            foreach (var prod in products)
            {

                listBox2.DisplayMember = "title";
                listBox2.ValueMember = "id";
                listBox2.Items.Add(new ProductBasic() { id = prod.id, title = prod.id + "-(" + prod.productSellerCode.ToString() + ")-" + prod.title });
                label2.Text = "Ürünler (" + products.Length.ToString() + ")";
            }
        }

        string seciliKategori;

        private void button2_Click_1(object sender, EventArgs e)
        {
            txtUrunKategoriNo.Text = seciliKategori;
        }

        private void lstBxUcuncuKategori_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (lstBxUcuncuKategori.SelectedItem == null)
            {
            }
            else
            {

                seciliKategori = lstBxUcuncuKategori.SelectedItem.ToString().Substring(0, 7);
            }
        }

        private void urunResimlericmbBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var client = new WebClient())
            {
                string hreflink = urunResimlericmbBox.SelectedItem.ToString();
                Uri uri = new Uri(hreflink);
                string filename = System.IO.Path.GetFileName(uri.LocalPath);
                //if (uri.IsFile)
                //{
                //    string filename1 = System.IO.Path.GetFileName(uri.LocalPath);
                //}
                hreflink.Replace("https", "http");
                client.DownloadFile(hreflink, filename);
                Form2 imageBox = new Form2(this.urunResimlericmbBox.SelectedItem.ToString());
                imageBox.Show();

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (Control txt in this.Controls)
                if (txt is TextBox || txt is ComboBox || txt is RichTextBox)
                    txt.Text = "";
            urunResimlericmbBox.Items.Clear();
            urunResimlericmbBox.SelectedIndex = -1;
            urunResimlericmbBox.Text = "";
            txtprodPrepDay.Text = "";
            txtUrunDurumu.Text = "";
            txtKargoSablonu.Text = "";
            txtStokMiktari.Text = "";
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            var authentication1 = new com.n11.api.Authentication();
            authentication1.appKey = "0269671d-74f9-492e-8ab4-8d3825c48715"; //api anahtarınız
            authentication1.appSecret = "VOZHd5LSaHXDdJXZ";//api şifeniz

            ProductServicePortService prodServ = new ProductServicePortService();



        }

        int prImgAdet = 1;

        private void button5_Click(object sender, EventArgs e)
        {

            ProductImage prImg = new ProductImage();
            ProductImage[] pr = new ProductImage[prImgAdet];
            string resimUrl = urunResimlericmbBox.Text.Replace("https", "http");
            prImg.url = resimUrl;

            prImg.order = txtBxUrunResimOrder.Text;

            urunResimlericmbBox.Items.Add(prImg);

            prImgAdet++;


        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            //apiAnahtari = textBox2.Text;
            //apiSifresi = textBox3.Text;
        }
    }
}
