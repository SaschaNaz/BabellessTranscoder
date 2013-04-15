using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using System.Text;
using System.Threading.Tasks;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Text_Transcoder
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : Text_Transcoder.Common.LayoutAwarePage
    {
        struct EncodingInfo
        {
            public UInt16 CodePage;
            public String Name;
            public String DisplayName;
        }
        static Int32 compareEncodingInfoByCodePage(EncodingInfo e1, EncodingInfo e2)
        {
            Int32 cp1 = e1.CodePage;
            Int32 cp2 = e2.CodePage;
            if (cp1 > cp2)
                return 1;
            else if (cp1 == cp2)
                return 0;
            else return -1;
        }
        static Int32 compareEncodingInfoByName(EncodingInfo e1, EncodingInfo e2)
        {
            return e1.Name.CompareTo(e2.Name);
        }
        static Int32 compareEncodingInfoByDisplayName(EncodingInfo e1, EncodingInfo e2)
        {
            return e1.DisplayName.CompareTo(e2.DisplayName);
        }
        List<EncodingInfo> encodinglist;

        public MainPage()
        {
            this.InitializeComponent();
            #region Add Encodings
            /* http://msdn.microsoft.com/en-us/library/windows/apps/system.text.encoding.aspx
             * exported by this code:
             * var pr1 = document.querySelectorAll(".tableSection>table>tbody>tr>td:nth-of-type(1)>p");
             * var pr2 = document.querySelectorAll(".tableSection>table>tbody>tr>td:nth-of-type(2)>p");
             * var pr3 = document.querySelectorAll(".tableSection>table>tbody>tr>td:nth-of-type(3)>p");
             * var str = "";
             * for (var i = 0; i < pr.length; i++) {
             *     str += "new EncodingInfo() { CodePage = " + pr1[i].innerHTML.trim() + ", Name = \"" + pr2[i].innerHTML.trim() + "\", DisplayName = \"" + pr3[i].innerHTML.trim() + "\" },\r\n";  }
             * console.log(str);
             */
            EncodingInfo[] encodings = new EncodingInfo[]
            {
                new EncodingInfo() { CodePage = 37, Name = "IBM037", DisplayName = "IBM EBCDIC (US-Canada)" },
                new EncodingInfo() { CodePage = 437, Name = "IBM437", DisplayName = "OEM United States" },
                new EncodingInfo() { CodePage = 500, Name = "IBM500", DisplayName = "IBM EBCDIC (International)" },
                new EncodingInfo() { CodePage = 708, Name = "ASMO-708", DisplayName = "Arabic (ASMO 708)" },
                new EncodingInfo() { CodePage = 720, Name = "DOS-720", DisplayName = "Arabic (DOS)" },
                new EncodingInfo() { CodePage = 737, Name = "ibm737", DisplayName = "Greek (DOS)" },
                new EncodingInfo() { CodePage = 775, Name = "ibm775", DisplayName = "Baltic (DOS)" },
                new EncodingInfo() { CodePage = 850, Name = "ibm850", DisplayName = "Western European (DOS)" },
                new EncodingInfo() { CodePage = 852, Name = "ibm852", DisplayName = "Central European (DOS)" },
                new EncodingInfo() { CodePage = 855, Name = "IBM855", DisplayName = "OEM Cyrillic" },
                new EncodingInfo() { CodePage = 857, Name = "ibm857", DisplayName = "Turkish (DOS)" },
                new EncodingInfo() { CodePage = 858, Name = "IBM00858", DisplayName = "OEM Multilingual Latin I" },
                new EncodingInfo() { CodePage = 860, Name = "IBM860", DisplayName = "Portuguese (DOS)" },
                new EncodingInfo() { CodePage = 861, Name = "ibm861", DisplayName = "Icelandic (DOS)" },
                new EncodingInfo() { CodePage = 862, Name = "DOS-862", DisplayName = "Hebrew (DOS)" },
                new EncodingInfo() { CodePage = 863, Name = "IBM863", DisplayName = "French Canadian (DOS)" },
                new EncodingInfo() { CodePage = 864, Name = "IBM864", DisplayName = "Arabic (864)" },
                new EncodingInfo() { CodePage = 865, Name = "IBM865", DisplayName = "Nordic (DOS)" },
                new EncodingInfo() { CodePage = 866, Name = "cp866", DisplayName = "Cyrillic (DOS)" },
                new EncodingInfo() { CodePage = 869, Name = "ibm869", DisplayName = "Greek, Modern (DOS)" },
                new EncodingInfo() { CodePage = 870, Name = "IBM870", DisplayName = "IBM EBCDIC (Multilingual Latin-2)" },
                new EncodingInfo() { CodePage = 874, Name = "windows-874", DisplayName = "Thai (Windows)" },
                new EncodingInfo() { CodePage = 875, Name = "cp875", DisplayName = "IBM EBCDIC (Greek Modern)" },
                new EncodingInfo() { CodePage = 932, Name = "shift_jis", DisplayName = "Japanese (Shift-JIS)" },
                new EncodingInfo() { CodePage = 936, Name = "gb2312", DisplayName = "Chinese Simplified (GB2312)" },
                new EncodingInfo() { CodePage = 949, Name = "ks_c_5601-1987", DisplayName = "Korean" },
                new EncodingInfo() { CodePage = 950, Name = "big5", DisplayName = "Chinese Traditional (Big5)" },
                new EncodingInfo() { CodePage = 1026, Name = "IBM1026", DisplayName = "IBM EBCDIC (Turkish Latin-5)" },
                new EncodingInfo() { CodePage = 1047, Name = "IBM01047", DisplayName = "IBM Latin-1" },
                new EncodingInfo() { CodePage = 1140, Name = "IBM01140", DisplayName = "IBM EBCDIC (US-Canada-Euro)" },
                new EncodingInfo() { CodePage = 1141, Name = "IBM01141", DisplayName = "IBM EBCDIC (Germany-Euro)" },
                new EncodingInfo() { CodePage = 1142, Name = "IBM01142", DisplayName = "IBM EBCDIC (Denmark-Norway-Euro)" },
                new EncodingInfo() { CodePage = 1143, Name = "IBM01143", DisplayName = "IBM EBCDIC (Finland-Sweden-Euro)" },
                new EncodingInfo() { CodePage = 1144, Name = "IBM01144", DisplayName = "IBM EBCDIC (Italy-Euro)" },
                new EncodingInfo() { CodePage = 1145, Name = "IBM01145", DisplayName = "IBM EBCDIC (Spain-Euro)" },
                new EncodingInfo() { CodePage = 1146, Name = "IBM01146", DisplayName = "IBM EBCDIC (UK-Euro)" },
                new EncodingInfo() { CodePage = 1147, Name = "IBM01147", DisplayName = "IBM EBCDIC (France-Euro)" },
                new EncodingInfo() { CodePage = 1148, Name = "IBM01148", DisplayName = "IBM EBCDIC (International-Euro)" },
                new EncodingInfo() { CodePage = 1149, Name = "IBM01149", DisplayName = "IBM EBCDIC (Icelandic-Euro)" },
                new EncodingInfo() { CodePage = 1200, Name = "utf-16", DisplayName = "Unicode" },
                new EncodingInfo() { CodePage = 1201, Name = "unicodeFFFE", DisplayName = "Unicode (Big endian)" },
                new EncodingInfo() { CodePage = 1250, Name = "windows-1250", DisplayName = "Central European (Windows)" },
                new EncodingInfo() { CodePage = 1251, Name = "windows-1251", DisplayName = "Cyrillic (Windows)" },
                new EncodingInfo() { CodePage = 1252, Name = "Windows-1252", DisplayName = "Western European (Windows)" },
                new EncodingInfo() { CodePage = 1253, Name = "windows-1253", DisplayName = "Greek (Windows)" },
                new EncodingInfo() { CodePage = 1254, Name = "windows-1254", DisplayName = "Turkish (Windows)" },
                new EncodingInfo() { CodePage = 1255, Name = "windows-1255", DisplayName = "Hebrew (Windows)" },
                new EncodingInfo() { CodePage = 1256, Name = "windows-1256", DisplayName = "Arabic (Windows)" },
                new EncodingInfo() { CodePage = 1257, Name = "windows-1257", DisplayName = "Baltic (Windows)" },
                new EncodingInfo() { CodePage = 1258, Name = "windows-1258", DisplayName = "Vietnamese (Windows)" },
                new EncodingInfo() { CodePage = 1361, Name = "Johab", DisplayName = "Korean (Johab)" },
                new EncodingInfo() { CodePage = 10000, Name = "macintosh", DisplayName = "Western European (Mac)" },
                new EncodingInfo() { CodePage = 10001, Name = "x-mac-japanese", DisplayName = "Japanese (Mac)" },
                new EncodingInfo() { CodePage = 10002, Name = "x-mac-chinesetrad", DisplayName = "Chinese Traditional (Mac)" },
                new EncodingInfo() { CodePage = 10003, Name = "x-mac-korean", DisplayName = "Korean (Mac)" },
                new EncodingInfo() { CodePage = 10004, Name = "x-mac-arabic", DisplayName = "Arabic (Mac)" },
                new EncodingInfo() { CodePage = 10005, Name = "x-mac-hebrew", DisplayName = "Hebrew (Mac)" },
                new EncodingInfo() { CodePage = 10006, Name = "x-mac-greek", DisplayName = "Greek (Mac)" },
                new EncodingInfo() { CodePage = 10007, Name = "x-mac-cyrillic", DisplayName = "Cyrillic (Mac)" },
                new EncodingInfo() { CodePage = 10008, Name = "x-mac-chinesesimp", DisplayName = "Chinese Simplified (Mac)" },
                new EncodingInfo() { CodePage = 10010, Name = "x-mac-romanian", DisplayName = "Romanian (Mac)" },
                new EncodingInfo() { CodePage = 10017, Name = "x-mac-ukrainian", DisplayName = "Ukrainian (Mac)" },
                new EncodingInfo() { CodePage = 10021, Name = "x-mac-thai", DisplayName = "Thai (Mac)" },
                new EncodingInfo() { CodePage = 10029, Name = "x-mac-ce", DisplayName = "Central European (Mac)" },
                new EncodingInfo() { CodePage = 10079, Name = "x-mac-icelandic", DisplayName = "Icelandic (Mac)" },
                new EncodingInfo() { CodePage = 10081, Name = "x-mac-turkish", DisplayName = "Turkish (Mac)" },
                new EncodingInfo() { CodePage = 10082, Name = "x-mac-croatian", DisplayName = "Croatian (Mac)" },
                new EncodingInfo() { CodePage = 12000, Name = "utf-32", DisplayName = "Unicode (UTF-32)" },
                new EncodingInfo() { CodePage = 12001, Name = "utf-32BE", DisplayName = "Unicode (UTF-32 Big endian)" },
                new EncodingInfo() { CodePage = 20000, Name = "x-Chinese-CNS", DisplayName = "Chinese Traditional (CNS)" },
                new EncodingInfo() { CodePage = 20001, Name = "x-cp20001", DisplayName = "TCA Taiwan" },
                new EncodingInfo() { CodePage = 20002, Name = "x-Chinese-Eten", DisplayName = "Chinese Traditional (Eten)" },
                new EncodingInfo() { CodePage = 20003, Name = "x-cp20003", DisplayName = "IBM5550 Taiwan" },
                new EncodingInfo() { CodePage = 20004, Name = "x-cp20004", DisplayName = "TeleText Taiwan" },
                new EncodingInfo() { CodePage = 20005, Name = "x-cp20005", DisplayName = "Wang Taiwan" },
                new EncodingInfo() { CodePage = 20105, Name = "x-IA5", DisplayName = "Western European (IA5)" },
                new EncodingInfo() { CodePage = 20106, Name = "x-IA5-German", DisplayName = "German (IA5)" },
                new EncodingInfo() { CodePage = 20107, Name = "x-IA5-Swedish", DisplayName = "Swedish (IA5)" },
                new EncodingInfo() { CodePage = 20108, Name = "x-IA5-Norwegian", DisplayName = "Norwegian (IA5)" },
                new EncodingInfo() { CodePage = 20127, Name = "us-ascii", DisplayName = "US-ASCII" },
                new EncodingInfo() { CodePage = 20261, Name = "x-cp20261", DisplayName = "T.61" },
                new EncodingInfo() { CodePage = 20269, Name = "x-cp20269", DisplayName = "ISO-6937" },
                new EncodingInfo() { CodePage = 20273, Name = "IBM273", DisplayName = "IBM EBCDIC (Germany)" },
                new EncodingInfo() { CodePage = 20277, Name = "IBM277", DisplayName = "IBM EBCDIC (Denmark-Norway)" },
                new EncodingInfo() { CodePage = 20278, Name = "IBM278", DisplayName = "IBM EBCDIC (Finland-Sweden)" },
                new EncodingInfo() { CodePage = 20280, Name = "IBM280", DisplayName = "IBM EBCDIC (Italy)" },
                new EncodingInfo() { CodePage = 20284, Name = "IBM284", DisplayName = "IBM EBCDIC (Spain)" },
                new EncodingInfo() { CodePage = 20285, Name = "IBM285", DisplayName = "IBM EBCDIC (UK)" },
                new EncodingInfo() { CodePage = 20290, Name = "IBM290", DisplayName = "IBM EBCDIC (Japanese katakana)" },
                new EncodingInfo() { CodePage = 20297, Name = "IBM297", DisplayName = "IBM EBCDIC (France)" },
                new EncodingInfo() { CodePage = 20420, Name = "IBM420", DisplayName = "IBM EBCDIC (Arabic)" },
                new EncodingInfo() { CodePage = 20423, Name = "IBM423", DisplayName = "IBM EBCDIC (Greek)" },
                new EncodingInfo() { CodePage = 20424, Name = "IBM424", DisplayName = "IBM EBCDIC (Hebrew)" },
                new EncodingInfo() { CodePage = 20833, Name = "x-EBCDIC-KoreanExtended", DisplayName = "IBM EBCDIC (Korean Extended)" },
                new EncodingInfo() { CodePage = 20838, Name = "IBM-Thai", DisplayName = "IBM EBCDIC (Thai)" },
                new EncodingInfo() { CodePage = 20866, Name = "koi8-r", DisplayName = "Cyrillic (KOI8-R)" },
                new EncodingInfo() { CodePage = 20871, Name = "IBM871", DisplayName = "IBM EBCDIC (Icelandic)" },
                new EncodingInfo() { CodePage = 20880, Name = "IBM880", DisplayName = "IBM EBCDIC (Cyrillic Russian)" },
                new EncodingInfo() { CodePage = 20905, Name = "IBM905", DisplayName = "IBM EBCDIC (Turkish)" },
                new EncodingInfo() { CodePage = 20924, Name = "IBM00924", DisplayName = "IBM Latin-1" },
                new EncodingInfo() { CodePage = 20932, Name = "EUC-JP", DisplayName = "Japanese (JIS 0208-1990 and 0212-1990)" },
                new EncodingInfo() { CodePage = 20936, Name = "x-cp20936", DisplayName = "Chinese Simplified (GB2312-80)" },
                new EncodingInfo() { CodePage = 20949, Name = "x-cp20949", DisplayName = "Korean Wansung" },
                new EncodingInfo() { CodePage = 21025, Name = "cp1025", DisplayName = "IBM EBCDIC (Cyrillic Serbian-Bulgarian)" },
                new EncodingInfo() { CodePage = 21866, Name = "koi8-u", DisplayName = "Cyrillic (KOI8-U)" },
                new EncodingInfo() { CodePage = 28591, Name = "iso-8859-1", DisplayName = "Western European (ISO)" },
                new EncodingInfo() { CodePage = 28592, Name = "iso-8859-2", DisplayName = "Central European (ISO)" },
                new EncodingInfo() { CodePage = 28593, Name = "iso-8859-3", DisplayName = "Latin 3 (ISO)" },
                new EncodingInfo() { CodePage = 28594, Name = "iso-8859-4", DisplayName = "Baltic (ISO)" },
                new EncodingInfo() { CodePage = 28595, Name = "iso-8859-5", DisplayName = "Cyrillic (ISO)" },
                new EncodingInfo() { CodePage = 28596, Name = "iso-8859-6", DisplayName = "Arabic (ISO)" },
                new EncodingInfo() { CodePage = 28597, Name = "iso-8859-7", DisplayName = "Greek (ISO)" },
                new EncodingInfo() { CodePage = 28598, Name = "iso-8859-8", DisplayName = "Hebrew (ISO-Visual)" },
                new EncodingInfo() { CodePage = 28599, Name = "iso-8859-9", DisplayName = "Turkish (ISO)" },
                new EncodingInfo() { CodePage = 28603, Name = "iso-8859-13", DisplayName = "Estonian (ISO)" },
                new EncodingInfo() { CodePage = 28605, Name = "iso-8859-15", DisplayName = "Latin 9 (ISO)" },
                new EncodingInfo() { CodePage = 29001, Name = "x-Europa", DisplayName = "Europa" },
                new EncodingInfo() { CodePage = 38598, Name = "iso-8859-8-i", DisplayName = "Hebrew (ISO-Logical)" },
                new EncodingInfo() { CodePage = 50220, Name = "iso-2022-jp", DisplayName = "Japanese (JIS)" },
                new EncodingInfo() { CodePage = 50221, Name = "csISO2022JP", DisplayName = "Japanese (JIS-Allow 1 byte Kana)" },
                new EncodingInfo() { CodePage = 50222, Name = "iso-2022-jp", DisplayName = "Japanese (JIS-Allow 1 byte Kana - SO/SI)" },
                new EncodingInfo() { CodePage = 50225, Name = "iso-2022-kr", DisplayName = "Korean (ISO)" },
                new EncodingInfo() { CodePage = 50227, Name = "x-cp50227", DisplayName = "Chinese Simplified (ISO-2022)" },
                new EncodingInfo() { CodePage = 51932, Name = "euc-jp", DisplayName = "Japanese (EUC)" },
                new EncodingInfo() { CodePage = 51936, Name = "EUC-CN", DisplayName = "Chinese Simplified (EUC)" },
                new EncodingInfo() { CodePage = 51949, Name = "euc-kr", DisplayName = "Korean (EUC)" },
                new EncodingInfo() { CodePage = 52936, Name = "hz-gb-2312", DisplayName = "Chinese Simplified (HZ)" },
                new EncodingInfo() { CodePage = 54936, Name = "GB18030", DisplayName = "Chinese Simplified (GB18030)" },
                new EncodingInfo() { CodePage = 57002, Name = "x-iscii-de", DisplayName = "ISCII Devanagari" },
                new EncodingInfo() { CodePage = 57003, Name = "x-iscii-be", DisplayName = "ISCII Bengali" },
                new EncodingInfo() { CodePage = 57004, Name = "x-iscii-ta", DisplayName = "ISCII Tamil" },
                new EncodingInfo() { CodePage = 57005, Name = "x-iscii-te", DisplayName = "ISCII Telugu" },
                new EncodingInfo() { CodePage = 57006, Name = "x-iscii-as", DisplayName = "ISCII Assamese" },
                new EncodingInfo() { CodePage = 57007, Name = "x-iscii-or", DisplayName = "ISCII Oriya" },
                new EncodingInfo() { CodePage = 57008, Name = "x-iscii-ka", DisplayName = "ISCII Kannada" },
                new EncodingInfo() { CodePage = 57009, Name = "x-iscii-ma", DisplayName = "ISCII Malayalam" },
                new EncodingInfo() { CodePage = 57010, Name = "x-iscii-gu", DisplayName = "ISCII Gujarati" },
                new EncodingInfo() { CodePage = 57011, Name = "x-iscii-pa", DisplayName = "ISCII Punjabi" },
                new EncodingInfo() { CodePage = 65000, Name = "utf-7", DisplayName = "Unicode (UTF-7)" },
                new EncodingInfo() { CodePage = 65001, Name = "utf-8", DisplayName = "Unicode (UTF-8)" }
            };
            encodinglist = encodings.ToList();
            #endregion
            RadioLN.IsChecked = true;
            //System.Text.Encoding.GetEncoding(
        }

        Comparison<EncodingInfo> comparisonMethodSelected;
        void Sort(Comparison<EncodingInfo> comparison)
        {
            Nullable<EncodingInfo> selectedFromEncoding = null;
            Nullable<EncodingInfo> selectedToEncoding = null;
            if (comparisonMethodSelected != null)
            {
                if (FromEncoding.SelectedIndex != -1)
                {
                    if (comparisonMethodSelected == compareEncodingInfoByCodePage)
                        selectedFromEncoding = encodinglist.Find(delegate(EncodingInfo info) { if (info.CodePage == (UInt16)FromEncoding.SelectedItem) return true; else return false; });
                    else if (comparisonMethodSelected == compareEncodingInfoByDisplayName)
                        selectedFromEncoding = encodinglist.Find(delegate(EncodingInfo info) { if (info.DisplayName == (String)FromEncoding.SelectedItem) return true; else return false; });
                    else if (comparisonMethodSelected == compareEncodingInfoByName)
                        selectedFromEncoding = encodinglist.Find(delegate(EncodingInfo info) { if (info.Name == (String)FromEncoding.SelectedItem) return true; else return false; });
                }
                if (ToEncoding.SelectedIndex != -1)
                {
                    if (comparisonMethodSelected == compareEncodingInfoByCodePage)
                        selectedToEncoding = encodinglist.Find(delegate(EncodingInfo info) { if (info.CodePage == (UInt16)ToEncoding.SelectedItem) return true; else return false; });
                    else if (comparisonMethodSelected == compareEncodingInfoByDisplayName)
                        selectedToEncoding = encodinglist.Find(delegate(EncodingInfo info) { if (info.DisplayName == (String)ToEncoding.SelectedItem) return true; else return false; });
                    else if (comparisonMethodSelected == compareEncodingInfoByName)
                        selectedToEncoding = encodinglist.Find(delegate(EncodingInfo info) { if (info.Name == (String)ToEncoding.SelectedItem) return true; else return false; });
                }
            }
                
            encodinglist.Sort(comparison);
            FromEncoding.Items.Clear();
            ToEncoding.Items.Clear();

            if (comparison == compareEncodingInfoByCodePage)
            {
                foreach(EncodingInfo info in encodinglist)
                {
                    FromEncoding.Items.Add(info.CodePage);
                    ToEncoding.Items.Add(info.CodePage);
                }
            }
            else if (comparison == compareEncodingInfoByDisplayName)
            {
                foreach(EncodingInfo info in encodinglist)
                {
                    FromEncoding.Items.Add(info.DisplayName);
                    ToEncoding.Items.Add(info.DisplayName);
                }
            }
            else if (comparison == compareEncodingInfoByName)
            {
                foreach (EncodingInfo info in encodinglist)
                {
                    FromEncoding.Items.Add(info.Name);
                    ToEncoding.Items.Add(info.Name);
                }
            }

            if (selectedFromEncoding != null)
            {
                if (comparison == compareEncodingInfoByCodePage)
                    FromEncoding.SelectedItem = selectedFromEncoding.Value.CodePage;
                else if (comparison == compareEncodingInfoByDisplayName)
                    FromEncoding.SelectedItem = selectedFromEncoding.Value.DisplayName;
                else if (comparison == compareEncodingInfoByName)
                    FromEncoding.SelectedItem = selectedFromEncoding.Value.Name;
            }
            if (selectedToEncoding != null)
            {
                if (comparison == compareEncodingInfoByCodePage)
                    ToEncoding.SelectedItem = selectedToEncoding.Value.CodePage;
                else if (comparison == compareEncodingInfoByDisplayName)
                    ToEncoding.SelectedItem = selectedToEncoding.Value.DisplayName;
                else if (comparison == compareEncodingInfoByName)
                    ToEncoding.SelectedItem = selectedToEncoding.Value.Name;
            }
            comparisonMethodSelected = comparison;
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void RadioButtonCP_Checked(object sender, RoutedEventArgs e)
        {
            Sort(compareEncodingInfoByCodePage);
        }

        private void RadioButtonLN_Checked(object sender, RoutedEventArgs e)
        {
            Sort(compareEncodingInfoByDisplayName);
        }

        private void RadioButtonSN_Checked(object sender, RoutedEventArgs e)
        {
            Sort(compareEncodingInfoByName);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //http://www.johnpapa.net/binding-to-silverlight-combobox-and-using-selectedvalue-selectedvaluepath-and-displaymemberpath/
            //이걸로 바인딩 붙여서 개선하기...
            //EncodingInfo 직접 ComboBox에 넣도록.
            //그에 따라 관련 코드 다 개선

            if (FromEncoding.SelectedItem == ToEncoding.SelectedItem)
            {
                var dialog = new Windows.UI.Popups.MessageDialog("Selected encodings are the same. Will you continue?");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Yes", null));
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("No", null, "no"));
                var result = await dialog.ShowAsync();
                if ((String)result.Id == "no")
                    return;
            }
            FileOpenPicker picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.List;
            Boolean isErrorOccured = false;
            try
            {
                foreach (String extension in fileExtensionBox.Text.Split(','))
                    picker.FileTypeFilter.Add(extension);
            }
            catch (ArgumentException)
            {
                isErrorOccured = true;
            }
            if (isErrorOccured)
            {
                await new Windows.UI.Popups.MessageDialog("Invalid file extensions are written in the text box. Keep the format, please").ShowAsync();
            }
            Boolean bigFileExist = false;
            var files = await picker.PickMultipleFilesAsync();
            foreach (StorageFile file in files)
            {
                using (var stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    if (stream.Size >= 4096)
                    {
                        bigFileExist = true;
                        break;
                    }
                }
            }
            if (bigFileExist == null)
            {
                var dialog = new Windows.UI.Popups.MessageDialog("One of the file is bigger than 4MB. There is a possibility that there is the file which is not a text. Will you continue?");
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Yes", null));
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("No", null, "no"));
                var result = await dialog.ShowAsync();
                if ((String)result.Id == "no")
                    return;
            }

            Parallel.ForEach(files,
                async delegate(StorageFile file)
                {
                    using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        using (DataReader reader = new DataReader(stream))
                        {
                            Byte[] bytes = new Byte[stream.Size];
                            await reader.LoadAsync((UInt32)stream.Size);
                            reader.ReadBytes(bytes);

                            //var euckr = System.Text.Encoding.GetEncoding("EUC-KR");
                            //var unibytes = System.Text.Encoding.Convert(euckr, System.Text.Encoding.Unicode, bytes);
                            //UnicodeSAMIDocument = System.Text.Encoding.Unicode.GetString(unibytes, 0, unibytes.Length);
                        }
                    }
                });
        }

        private void EncodingSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FromEncoding.SelectedIndex != -1 && ToEncoding.SelectedIndex != -1)
                loadButton.IsEnabled = true;
            else loadButton.IsEnabled = false;
        }
    }
}
