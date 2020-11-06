using Kurs_2_Uppgift_1_VG.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Kurs_2_Uppgift_1_VG
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
     
        public MainPage()
        {
            this.InitializeComponent();
       
        }
        private  async Task CreateFileAsync()
        {
          
            StorageFolder storageFolder = KnownFolders.DocumentsLibrary;

            switch (cbFormat.SelectedIndex)
            {
                case 0:
                    await storageFolder.CreateFileAsync($"File.json", CreationCollisionOption.ReplaceExisting);
                    StorageFile jsonFile = await storageFolder.GetFileAsync("File.json");
                    await FileIO.WriteTextAsync(jsonFile, JsonConvert.SerializeObject(new Person { FirstName = tbxFirst.Text, LastName = tbxLast.Text, Age = Convert.ToInt32(tbxAge.Text), City = tbxCity.Text }));
                    var textFromJsonFile = await FileIO.ReadTextAsync(jsonFile);
                    lwFiles.Items.Add(textFromJsonFile);
                    ClearTextBoxes();
                    break;

                case 1:
                    await storageFolder.CreateFileAsync($"File.csv", CreationCollisionOption.ReplaceExisting);
                   
                        StorageFile csvFile = await storageFolder.GetFileAsync("File.csv");
                        char delimiter = ';';
                        var content = tbxFirst.Text + delimiter + tbxLast.Text + delimiter + Convert.ToInt32(tbxAge.Text) + delimiter + tbxCity.Text;
                        var lines = new List<string> { content };
                        await FileIO.AppendLinesAsync(csvFile, lines);
                        var textFromCsvFile = await FileIO.ReadTextAsync(csvFile);
                        lwFiles.Items.Add(textFromCsvFile);
                        ClearTextBoxes();
                    
                  
                    break;
                case 2:
                    await storageFolder.CreateFileAsync($"File.xml", CreationCollisionOption.ReplaceExisting);
                    StorageFile file = await storageFolder.GetFileAsync("File.xml");
                    using(var stream = await file.OpenStreamForWriteAsync())
                    {
                        XmlWriterSettings settings = new XmlWriterSettings()
                        {
                            Indent = true,
                            IndentChars = ("  "),
                            CloseOutput = true
                        };
                        using XmlWriter xml = XmlWriter.Create(stream, settings);
                        xml.WriteStartElement("people");
                        xml.WriteStartElement("person");
                        xml.WriteAttributeString("firstname", tbxFirst.Text);
                        xml.WriteElementString("lastname", tbxLast.Text);
                        xml.WriteElementString("age",tbxAge.Text);
                        xml.WriteElementString("city", tbxCity.Text);
                        xml.WriteEndElement();
                        xml.WriteEndElement();
                        xml.Close();
                        var textFromXmlFile = await FileIO.ReadTextAsync(file);
                        lwFiles.Items.Add(textFromXmlFile);
                        ClearTextBoxes();
                    }
                    break;   

                case 3:
                    await storageFolder.CreateFileAsync($"File.txt", CreationCollisionOption.ReplaceExisting);
                    StorageFile txtFile = await storageFolder.GetFileAsync("File.txt");
                    var text = tbxFirst.Text + " " + tbxLast.Text + " " + Convert.ToInt32(tbxAge.Text) + " " + tbxCity.Text;
                    await FileIO.WriteTextAsync(txtFile, text);
                    var textFromTxtFile = await FileIO.ReadTextAsync(txtFile);
                    lwFiles.Items.Add(textFromTxtFile);
                    ClearTextBoxes();
                    break;
            }
        }
        private void ClearTextBoxes()
        {
            tbxFirst.Text = "";
            tbxLast.Text = "";
            tbxAge.Text = "";
            tbxCity.Text = "";
            cbFormat.SelectedIndex = 3;
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
           await CreateFileAsync();
        }
    }
}
