using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Yandex.Cloud;
using Yandex.Cloud.Credentials;

namespace UseLibrary
{
    public partial class Form1 : Form
    {
        CalcValut.calcValut calc = new CalcValut.calcValut();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //используем калькулятор из библиотеки написанной "дргуим программистом"
            double res = calc.kalkulateKurs(100,CalcValut.valut.USD);
            MessageBox.Show(res.ToString());
        }
        YandexTranslator tr;
        private void Form1_Load(object sender, EventArgs e)
        {
            tr = new YandexTranslator();
            foreach (var item in tr.langs())
            {
                comboBox1.Items.Add(item);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            string langfrom = "ru";
            string langto = comboBox1.SelectedItem.ToString();
            var res = tr.translate(langfrom, langto, text);
            MessageBox.Show(res);
        }
    }
    class YandexTranslator
    {
        private Sdk yasdk;
        private string Oauth = "";
        private string folderID = "";
        public YandexTranslator()
        {
            yasdk = new Sdk(new OAuthCredentialsProvider(Oauth));
        }
        public string translate(string langfrom, string langto, params string[] texts)
        {
            //yandex translation example

            var req = new Yandex.Cloud.Ai.Translate.V2.TranslateRequest();
            req.FolderId = folderID;
            req.TargetLanguageCode = langto;
            req.SourceLanguageCode = langfrom;
            foreach (var text in texts)
            {
                req.Texts.Add(text);
            }

            var ans = yasdk.Services.Ai.Translate.TranslationService.Translate(req);
            string res = String.Join(" ", ans.Translations);

            return res;
        }
        public List<string> langs()
        {
            var req = new Yandex.Cloud.Ai.Translate.V2.ListLanguagesRequest();
            req.FolderId = folderID;
            var available = yasdk.Services.Ai.Translate.TranslationService.ListLanguages(req);
            List<string> res = new List<string>();
            foreach (var lang in available.Languages)
            {
                res.Add(lang.Code);
            }
            return res;
        }
    }
}
