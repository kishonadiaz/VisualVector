using Microsoft.Web.WebView2.Core;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Wpf;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using Newtonsoft.Json;
using HandyControl.Tools.Extension;
using Microsoft.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Reflection;
using Microsoft.VisualBasic;
using System.Collections;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;


namespace InterviewProp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();

            Loaded += Main_onload;
        }

        private async void Main_onload(object sender, RoutedEventArgs e)
        {

            
            CoreWebView2EnvironmentOptions options = new CoreWebView2EnvironmentOptions("--disable-web-security");
            CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync("", "", options);
            await WebView.EnsureCoreWebView2Async(environment);
            string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;


            string startupPath = System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName+"/");
            var path = new Uri(startupPath, UriKind.Absolute);
            Debug.WriteLine(path.AbsoluteUri);

            WebView.CoreWebView2.Settings.AreHostObjectsAllowed = true;
            WebView.CoreWebView2.Settings.IsScriptEnabled = true;

            WebView.CoreWebView2.SetVirtualHostNameToFolderMapping("interview.project", Environment.CurrentDirectory, CoreWebView2HostResourceAccessKind.Allow);

            WebView.Source = new Uri("https://interview.project/interviewThreejs.html");
           // WebView.Source = new Uri(path.AbsoluteUri);


            Debug.WriteLine(WebView.Source.ToString());
            WebView.CoreWebView2.WebResourceResponseReceived += CoreWebView2_WebResourceResponseReceived;
            WebView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
            /*var request = WebView.CoreWebView2.Environment.CreateWebResourceRequest(new Uri("https://interview.project/interviewThreejs.html"),
              "POST", postData, "Content-Type: application/x-www-form-urlencoded");*/
        }
        Dictionary<string,object> keyValuePairs = new Dictionary<string,object>();

        private async void ApiCall(Object arg)
        {
            Debug.WriteLine(arg.ToString());
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(arg.ToString());

            //var data = JsonConvert.SerializeObject(json);
            Debug.WriteLine(json);
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.meshy.ai");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "msy_Mbg1e6g4Ywe9mt5cWbs1nuw4zKwgwFCgXhDG");
            
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri("https://api.meshy.ai/v2/text-to-3d"));
            request.Content = new StringContent(arg.ToString(), Encoding.UTF8, "application/json");
           
            await client.SendAsync(request).ContinueWith(responseTask =>
            {
                Debug.WriteLine("Response: {0}", responseTask.Result);

            });
            var response = await client.PostAsync(new Uri("https://api.meshy.ai/v2/text-to-3d"), request.Content);
            var contents = await response.Content.ReadAsStringAsync();

            Debug.WriteLine(contents);
            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(contents);
            request = new HttpRequestMessage(HttpMethod.Post, new Uri("https://api.meshy.ai/v2/text-to-3d/" + data?["result"].ToString()+ "?api_key=msy_Mbg1e6g4Ywe9mt5cWbs1nuw4zKwgwFCgXhDG"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "msy_Mbg1e6g4Ywe9mt5cWbs1nuw4zKwgwFCgXhDG");
            request.Content = new StringContent(data?["result"]+ "?api_key=msy_Mbg1e6g4Ywe9mt5cWbs1nuw4zKwgwFCgXhDG", Encoding.UTF8, "application /json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "msy_Mbg1e6g4Ywe9mt5cWbs1nuw4zKwgwFCgXhDG");
           
            response = await client.PostAsync(new Uri("https://api.meshy.ai/v2/text-to-3d/"), request.Content);
           
            contents = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(contents);
            /* var outtodata = "{\"mode\":\"refine\"}";*/

        }

        List<string> animationactions = new List<string>();
        List<bool> animebool = new List<bool>();
        List<int> animecount = new List<int>();
        List<string> editoractions = new List<string>();

        Dictionary<string, object> annotations = new Dictionary<string, object>
        {
            ["editoraction"] = new Dictionary<string, object>(),
            ["methodsname"] =new List<string>()
        };

        private string CSharpToJS(string item)
        {
            var syntax = new[] { "int", "decimal", "float", "void", "string", "String","var","const" ,"object","bool","let","long"};
            var scope = new[] { "public", "private", "protected" };
            int acount = animecount.Count;

            

            if(item.IndexOf("(") < item.IndexOf("Dictionary") && item.IndexOf(")") > item.IndexOf("Dictionary"))
            {
                if (item.Contains("Dictionary"))
                {
                    foreach (var item2 in syntax)
                    { 
                        if(item.IndexOf("<") < item.IndexOf(item2) && item.IndexOf(">") > item.IndexOf(item2))
                        {
                            item = item.Replace(item2, "");
                        }
                        if (item.IndexOf(">") < item.IndexOf(")"))
                        {
                            if (!item.Contains(item2))
                            {
                                var a = item.Substring((item.IndexOf(">") + 1),(Math.Abs((item.IndexOf(">") + 1)- (item.IndexOf(")")))));
                                if(!a.IsNullOrEmpty())
                                    item = item.Replace(a, "");
                            }
                        }
                      
                    }
                    if (item.IndexOf("<") > item.IndexOf("Dictionary"))
                    {
                        item = item.Replace("<", "");
                    }
                    if (item.IndexOf(">") < item.IndexOf(")"))
                    {
                        item = item.Replace(">", "").Trim();
                    }
                    
                    item = item.Replace("Dictionary", "obj");

                    while (item.IndexOf("(") < item.IndexOf(")") && item.Contains(","))
                    {

                        item = item.Replace(",", "");

                    }

                }
                
                foreach (var item2 in syntax)
                {
                   
                    while (item.ToString().Contains(item2))
                    {

                       
                        
                        item = item.Replace(item2, "window.");
                        item = item.Insert(item.IndexOf("("), "=function");
                        var a = item.Substring(item.IndexOf("window.") + Math.Abs(item.IndexOf(" ")), Math.Abs((item.IndexOf(" "))));
                        item = item.Replace(". ", ".");
                        
                        if (item2 == "decimal")
                        {
                            if(item.IndexOf("{") < item.IndexOf("}"))
                            {
                                for(int i = item.IndexOf("{"); i < item.IndexOf("}"); i++)
                                {
                                    if(item.IndexOf("0") != -1 && item.Contains("M"))
                                    {
                                        item = item.Replace("M", "");
                                    }
                                }
                            }
                        }
                       
                    }

                }
      
                foreach (var item2 in scope)
                {
                    while (item.ToString().Contains(item2))
                    {
                        item = item.Replace(item2, "");
                       
                    }
                }
                


            }
            string argsa = "";
            if (!item.Contains("Dictionary"))
            {
                foreach (var item2 in syntax)
                {
                    while (item.ToString().Contains(item2))
                    {
                        if (item.ToString().Contains("class"))
                        {   
                            
                            if (item.ToString().Contains("(") && item.ToString().Contains(")"))
                            {
                                item = item.Replace(item2, "");

                            }

                        }
                        else
                        {
                            if (item.IndexOf("(") < item.IndexOf(")"))
                            {
                                for (var i = item.IndexOf("(") + 1; i < item.IndexOf(")"); i++)
                                {
                                    argsa += item[i].ToString();

                                }
                               
                                item = item.Replace(argsa, "~");
                                foreach (var i in syntax)
                                {
                                    while (argsa.Contains(i))
                                    {
                                       
                                        argsa = argsa.Replace(i, "");
                                    }
                                }
                                item = item.Replace("~", argsa);
                                
                                

                              
                                Debug.WriteLine(argsa);
                                
                            }
                            item = item.Replace(item2, "window.");
                            while (item.Contains(". "))
                            {
                                item = item.Replace(". ", ".");
                            }

                        }

                        
                        
                    }
                }
                while (!item.Contains("=function") && item.IndexOf("\n{") != -1)
                {
                    item = item.Insert(Math.Abs(item.IndexOf("(")), "=function");
                    if (item.Contains("}"))
                        item = item.Insert(Math.Abs(item.LastIndexOf("}")+1 ), "\n");
                }
                foreach (var item2 in scope)
                {
                    while (item.ToString().Contains(item2))
                    {
                        item = item.Replace(item2, "");
                    }
                }
            }
            List<string> leftp = new List<string>();
            List<string> rightp = new List<string>();
            var test = item.ToString().Split("\n");
            int lcount = 0;
            int rcount=0;
            int ldcount = 0;
            string outsf = item.ToString();
           
            foreach (var item2 in test) { 
                Debug.WriteLine(item2.Trim());
                for (int i = 0; i < item2.Length; i++)
                {
                    Debug.WriteLine(item2[i]);
                    if (item2[i].ToString() == "(")
                    {
                        lcount++;
                    }
                    if (item2[i].ToString() == ")")
                    {
                        rcount++;
                    }

                }
                if (lcount != rcount)
                {
                    Debug.WriteLine(lcount + ": " + rcount);
                    outsf = item2.Insert(item2.Length,")");
                    
                }

                string[] outsf1 = new[] { outsf};
                test = outsf1;

            }
            foreach (var item2 in test)
            {
                if (!item2.Contains("\n"))
                    item = item2.Insert(item2.Length, "\n");
                else
                    item = item2;
                Debug.WriteLine(item2.Trim());

            }
            Debug.WriteLine(test);
            Debug.WriteLine(leftp.Count);
            Debug.WriteLine(rightp.Count);
            if (leftp.Count != rightp.Count)
            {
                item = item.Insert(item.LastIndexOf(")"), ")");
            }

            
            var k = Array.Empty<string>();
            if (animebool.Contains(true))
            {
                if (animebool[animecount.Count] == true)
                {
                    k = item.Substring(item.IndexOf(".") + 1, item.IndexOf("=")).Split("=");
                    Debug.WriteLine(k[0]);
                    animationactions.Add(k[0]);
                    animebool[animecount.Count] = false;
                    if ((annotations["editoraction"] as Dictionary<string, object>).Keys.Contains(editoractions[animecount.Count].Replace("\r", "").Replace("\n", ""))) 
                    {
                        (annotations["editoraction"] as Dictionary<string, object>)[editoractions[animecount.Count].Replace("\r","").Replace("\n", "")] = k[0];
                    }
                    else
                    {
                        (annotations["editoraction"] as Dictionary<string, object>).Add(editoractions[animecount.Count].Replace("\r", "").Replace("\n",""), k[0]);
                    }
                    
                    animecount.Add(animecount.Count+1);


                }
            }
            else
            {
                if (animecount.Count > 0)
                {
                    if (!animationactions[animecount.Count - 1].IsNullOrEmpty() && !editoractions[animecount.Count - 1].IsNullOrEmpty())
                    {
                        Debug.WriteLine(item);
                        Debug.WriteLine(animationactions[animecount.Count-1]);
                        if (editoractions.Contains(editoractions[animecount.Count - 1]) && animationactions.Contains(animationactions[animecount.Count - 1]))
                        {
                            k = animationactions.ToArray<string>();
                        }

                    }
                }
            }
            string animeaction = "";
            bool notthesame = false;
            if(item.Contains("/#") && item.Contains("#/"))
            {
                item = item.Replace("/#", "").Replace("#/", "");
                if (animecount.Count > 0)
                {
                    if (!animationactions[animecount.Count - 1].IsNullOrEmpty() && !editoractions[animecount.Count - 1].IsNullOrEmpty())
                    {
                        if(k.Length > 0) { 
                            Debug.WriteLine(k);
                            if (editoractions[animecount.Count-1].Contains(item) && animationactions[animecount.Count - 1].Contains(k[0]) )
                            {
                                notthesame = true;
                            }
                        }
                    }
                }
                if (!notthesame)
                {
                    animebool.Add(true);

                    editoractions.Add(item);
                    
                }
                item = item.Replace(item, "");

            }

            if (item.Contains("window."))
            {
                if ((annotations["methodsname"] as List<string>).Count > 0)
                {
                    if (!(annotations["methodsname"] as List<string>).Contains(item.Substring(item.IndexOf("window."), item.IndexOf("=")).Split(".")[1]))
                    {
                        (annotations["methodsname"] as List<string>).Add(item.Substring(item.IndexOf("window."), item.IndexOf("=")).Replace("=", "").Split(".")[1]);
                       /* if ((annotations["methodsname"] as List<string>)[(annotations["methodsname"] as List<string>).Count - 1].Contains(item.Substring(item.IndexOf("window."), item.IndexOf("=")).Split(".")[1]))*/



                    }
                    else
                        (annotations["methodsname"] as List<string>)[(annotations["methodsname"] as List<string>).Count-1] = item.Substring(item.IndexOf("window."), item.IndexOf("=")).Replace("=", "").Split(".")[1];
                }
                else
                {
                    (annotations["methodsname"] as List<string>).Add(item.Substring(item.IndexOf("window."), item.IndexOf("=")).Replace("=", "").Split(".")[1]);
                }
            }
            
           

            return item;

        }
        private string CSharpToJS(object item)
        {
            var str = item.ToString();
            var syntax = new[] { "int", "decimal", "float", "void", "string", "String","object","bool" };
            var scope = new[] { "public", "private", "protected" };
            var para = new[] { "," };
            if(str?.IndexOf("(") < str?.IndexOf("Dictionary") && str.IndexOf(")") > str.IndexOf("Dictionary"))
            {
                if (str.Contains("Dictionary"))
                {
                    foreach (var item2 in syntax)
                    { 
                        if(str.IndexOf("<") < str.IndexOf(item2) && str.IndexOf(">") > str.IndexOf(item2))
                        {
                            str = str.Replace(item2, "");
                        }
                        if (str.IndexOf(">") < str.IndexOf(")"))
                        {
                            if (!str.Contains(item2))
                            {
                                var a = str.Substring((str.IndexOf(">") + 1),(Math.Abs((str.IndexOf(">") + 1)- (str.IndexOf(")")))));
                                if(!a.IsNullOrEmpty())
                                    str = str.Replace(a, "");
                            }
                        }
                       /* if (item.IndexOf("(") < item.IndexOf(item2) && item.IndexOf(")") > item.IndexOf(item2))
                        {
                            item = item.Replace(item2, "");
                        }*/
                        /*if (item.IndexOf("(") < item.IndexOf(")"))
                        {
                            if (!item.Contains(item2))
                            {
                                Debug.WriteLine(item.Substring(Math.Abs((item.IndexOf(',') !=-1) ? item.IndexOf(",") + 1 : item.IndexOf("(") + 1)));
                                var a = item.Substring((item.IndexOf(",") + 1 | item.IndexOf("(") + 1), (Math.Abs((item.IndexOf(",") + 1 | item.IndexOf("(") + 1) - (item.IndexOf(")")))));
                                //if (!a.IsNullOrEmpty())
                                    //item = item.Replace(a, "");
                            }
                        }*/
                    }
                    if (str.IndexOf("<") > str.IndexOf("Dictionary"))
                    {
                        str = str.Replace("<", "");
                    }
                    if (str.IndexOf(">") < str.IndexOf(")"))
                    {
                        str = str.Replace(">", "").Trim();
                    }

                    str = str.Replace("Dictionary", "obj");

                   
                    
                }
                foreach (var item2 in syntax)
                {
                    while (str.Contains(item2))
                    {
                        str = str.Replace(item2, "var");
                    }
                }
                foreach (var item2 in scope)
                {
                    while (item.ToString().Contains(item2))
                    {
                        str = str.Replace(item2, "");
                    }
                }



            }
            List<bool> isconstructor = new List<bool>();
            int flagcount = 0;
          
            if (!str.Contains("Dictionary"))
            {
                foreach (var item2 in syntax)
                {
                    while (str.Contains(item2))
                    {
                        if (str.Contains("class"))
                        {   
                            if(str.Contains((item as ClassDeclarationSyntax).Identifier.ToString()))
                            {
                                if (str.Contains("\n{"))
                                {
                                    string strs = "";
                                    if (str.IndexOf((item as ClassDeclarationSyntax).Identifier.ToString()+"(") < str.IndexOf(")"))
                                    {
                                       /* for(int i = str.IndexOf("(")+1; i < str.IndexOf(")")-1; i++)
                                        {
                                            if (i > str.IndexOf("(") && i >= str.IndexOf(")")+1) break;
                                                Debug.WriteLine(str.Substring(i));
                                            str = str.Replace(str.Substring(i, str.IndexOf(")")), "");
                                        }
                                        str = str.Replace(item2, "").Trim();*/

                                    }
                                    //if (str.IndexOf((item as ClassDeclarationSyntax).Identifier.ToString()+"()\r\n {") < str.IndexOf("}"))
                                    //{
                                       
                                       
                                    //   /* if (str.LastIndexOf("}") > str.IndexOf("}"))
                                    //        continue;*/
                                    //    //var sf = str.Substring((str.IndexOf(">") + 1), (Math.Abs((str.IndexOf(">") + 1) - (str.IndexOf(")")))));
                                    //    Debug.WriteLine((item as ClassDeclarationSyntax).Identifier.ToString() + "()");
                                    //    str = str.Replace((item as ClassDeclarationSyntax).Identifier.ToString() + "()", "");
                                      

                                    //        str = str.Replace("{", "");
                                 
                                    //        str = str.Replace("}", "");

                                    //}
                                    
                                    //str = str.Replace((item as ClassDeclarationSyntax).Identifier.ToString(), "");

                                   
                                }
                                if (!str.Contains((item as ClassDeclarationSyntax).Identifier.ToString() + "()\n"))
                                {
                                    if (str.IndexOf((item as ClassDeclarationSyntax).Identifier.ToString()) < str.IndexOf("(") && str.IndexOf(")\n{") < str.IndexOf("}") )
                                    {
                                        if(str.IndexOf("(") < str.IndexOf("}"))
                                        {
                                            isconstructor.Add(true);
                                            flagcount++;
                                            //break;
                                        }
                                        /*var sf = str.ToCharArray();
                                        for (int i = 0;i < str.Length; i++)
                                        {
                                            for(int j =0; j < str.Length; j++)
                                            {
                                               // Debug.WriteLine(str[j]);
                                                if (str[j] == '{' || str[j] == '}')
                                                {
                                                    sf[j] = ' ';

                                                    
                                                }

                                            }
                                           
                                            
                                        }*/
                                        /*for (int i = 0; i < sf.Length; i++)
                                        {
                                            Debug.WriteLine(sf[i].ToString());


                                        }*/
                                     

                                    }
                                    var tempstr = "";
                                    int pcaounterf = 0;
                                    int pcaounterl = 0;
                                    int pinit = 0;
                                    var pDict = new Dictionary<int,Dictionary<string,Dictionary<string,int>>>();
                                    if(str.Contains((item as ClassDeclarationSyntax).Identifier.ToString() + "(")){
                                        /*str = str.Replace("{", "");
                                        str = str.Replace("}", "");*/
                                        for(var i= str.IndexOf("{"); i < str.IndexOf("}"); i++)
                                        {
                                            Debug.WriteLine(i);
                                           
                                            if (str[i] == '{')
                                            {
                                                pDict[pinit]["fp"+pinit] = new Dictionary<string, int> { { "fp", i } };
                                                pcaounterf++;
                                            }
                                            if (str[i] == '}')
                                            {
                                                pDict[pinit]["fe" + pinit] = new Dictionary<string, int> { { "fe", i } };
                                                pcaounterl++;
                                            }
                                            if (pcaounterf == pcaounterl)
                                            {
                                                
                                                pinit++;
                                            }


                                        }

                                        for(var i = 0; i < pinit; i++)
                                        {

                                        }
                                        
                                        Debug.WriteLine(str);
                                    }
                                    str = str.Replace((item as ClassDeclarationSyntax).Identifier.ToString(), "");
                                    /*for (int item3 =  0; item3 <  isconstructor.Count; item3++)
                                    {
                                        if (isconstructor[flagcount-1] == true)
                                        {
                                            str = str.Replace("{", "");
                                            str = str.Replace("}", "");
                                            isconstructor[flagcount-1] = false;
                                            break;
                                        }

                                    }*/
                                    string ss = "";
                                    for(int i = str.IndexOf("(")+1; i < str.IndexOf(")"); i++)
                                    {
                                        ss += str[i];
                                        if( i >= str.IndexOf(")"))
                                        {
                                            break;
                                        }
                                    }
                                   
                                    str = str.Replace("(" + ss + ")", "");
                                    
                                    for(int i = 0; i < syntax.Length; i++) 
                                    {
                                        ss = ss.Replace(syntax[i], "");
                                        
                                    }
                                    str = str.Replace("class", "window." + (item as ClassDeclarationSyntax).Identifier.ToString() + "=new function("+ss+")");
                                }


                                

                            }
                            

                        }
                        else
                        {   
                            str = str.Replace(item2, "this.");
                        }
                        
                    }
                }
                foreach (var item2 in scope)
                {
                    while (str.Contains(item2))
                    {
                        str = str.Replace(item2, "");
                    }
                }
                //str = str.Replace(str, "window.");
                while (str.Contains(". "))
                {
                    str = str.Replace(". ", ".");
                }
                Debug.WriteLine(str);
            }
            

            return str.TrimStart().Trim();

        }
    
        private async void EditorProcess(Dictionary<string,object> arg)
        {
            string data = arg?["data"].ToString();
            /*var codeToCompile = "using System;"+
"using System.Collections.Generic;"+
  "int test = 0;"+
  "var count = test + 15;"+
  "count++;"+
  "Dictionary<string,string> a = new Dictionary<string,string>();"+
   " a.Add(\"here\",\"there\");"+
  "return a;";*/ /*var codeToCompile = @"
    using System.Collections.Generic;
using System.Collections;
using System.Text;
using System;
using System.Diagnostics;

int j =0;
var watermelon = new Dictionary<string, object>
{
    [""Price""] =  2.00M,
     [""Color""] = ""green"",
    [""Source""] = new[]{""USA"",""Brazil""},
    [""Seedless""] = false
};

public decimal BuyFruit(Dictionary<string,object> fruitType)
{
    //Buy a fruit
    return 0.00M;
}

public void SeedFruit(Dictionary<string, object> fruitType)
{
    //Seed a fruit
}

";*/
            var codeToCompile = CSharpSyntaxTree.ParseText(@"
    using System.Collections.Generic;
    using System.Collections;
    using System.Text;
    using System;
    using System.Diagnostics;
    
class Cat
{

    int legs = 4;
    string color = ""orange"";
    
    Cat(int test, string jets)
    {
        int x = 0;
        float y  = 0.0
        int test = test
    }
    
    public bool isPet(){
        return true;
    }
}

");

            var tree= CSharpSyntaxTree.ParseText(@"
    using System.Collections.Generic;
using System.Collections;
using System.Text;
using System;

const geometry = new THREE.BoxGeometry(100, 100, 100);
    let materialArrays = [];
    materialArrays.push(new THREE.MeshPhongMaterial({ color: 0x00ff00 }))
    materialArrays.push(new THREE.MeshPhongMaterial({ color: new THREE.Color(0xfffff) }))
    materialArrays.push(new THREE.MeshPhongMaterial({ color: 0xffffff }))
    materialArrays.push(new THREE.MeshPhongMaterial({ color: 0x32f888 }))
    materialArrays.push(new THREE.MeshPhongMaterial({ color: 0x00ff00 }))
    materialArrays.push(new THREE.MeshPhongMaterial({ color: 0x00ff00 }))
    var cube = new THREE.Mesh(geometry, materialArrays);
    cube.position.set(0, 55, 0);


public decimal BuyFruit(Dictionary<string,object> fruitType)
{
    
    return 1.00M;
}


");
            var dataarg = CSharpSyntaxTree.ParseText(data);

            /*var syntaxRoot = tree.GetRoot();*/
            //var members = dataarg.GetRoot().DescendantNodes().OfType<MemberDeclarationSyntax>();
            var members = codeToCompile.GetRoot().DescendantNodes().OfType<MemberDeclarationSyntax>();
            var strarr = new ArrayList();
            string outs = "";
            foreach (var member in members)
            {
                var g = member as GlobalStatementSyntax;
                if (g != null)
                {
                    var s = g.ToString();
                    s = CSharpToJS(s);

                    Debug.WriteLine("global: " + s);
                    strarr.Add(s);
                    outs += s;
                }

                    
                var classs = member as ClassDeclarationSyntax;
                if (classs != null)
                {
                    var s = classs;
                    var d = CSharpToJS(s);

                    Debug.WriteLine("global: " + d);
                    outs += d;
                    /*Debug.WriteLine("Class: " + classs);*/
                }
                    
                var field = member as FieldDeclarationSyntax;
                if (field != null) { 
                }
                    Debug.WriteLine("Field: " + field);
                var property = member as PropertyDeclarationSyntax;
                if (property != null)
                    Debug.WriteLine("Property: " + property.Identifier);
                var method = member as MethodDeclarationSyntax;
                if (method != null)
                    Debug.WriteLine("Method: " + method.Identifier);
            }
         
           
           

            var dataout = new Dictionary<string, object>
            {
                ["scriptname"] = arg?["scriptname"].ToString(),
                ["data"] = outs,
                ["annotation"] = JsonConvert.SerializeObject( annotations)
            };
            Debug.WriteLine(dataout);
            
            WebView.CoreWebView2.PostWebMessageAsJson(JsonConvert.SerializeObject(dataout));
            annotations["methodsname"] = new List<string>();
            /*  var MyMethod = syntaxRoot.DescendantNodes().OfType<MethodDeclarationSyntax>().Where(n => n.ParameterList.Parameters.Any()).First();

              //Find the type that contains this method
              var containingType = MyMethod.Ancestors().OfType<TypeDeclarationSyntax>().First();

              Debug.WriteLine(containingType.Identifier.ToString());
              Debug.WriteLine(MyMethod.ToString());*/

            Debug.WriteLine(arg);
          /*  var options = ScriptOptions.Default;

            Script compiledScript = CSharpScript.Create<Dictionary<string,object>>(codeToCompile, options);

            compiledScript.Compile();
            
            var result = await compiledScript.RunAsync();
            
            var c = compiledScript.GetCompilation();

            //Debug.WriteLine(result?.Script.Code.ToString());
            var obj = new List<object>();
            var datas = new ArrayList();
            var sd = await CSharpScript.RunAsync(codeToCompile, ScriptOptions.Default);
            Debug.WriteLine(result.Script.ReturnType);
            foreach (var variable in result.Variables)
            {
                Debug.WriteLine(variable);
                obj.Add(variable);

                datas.Add(JsonConvert.SerializeObject(variable));
            }

            var data = JsonConvert.SerializeObject(obj);
            Debug.WriteLine(result);*/


            /*     var exec = new CSharpScriptExecution() { SaveGeneratedCode = true };
                 exec.AddDefaultReferencesAndNamespaces();*/

            /*            var code = $@"
            // pick up and cast parameters
            int num1 = (int) @0;   // same as parameters[0];
            int num2 = (int) @1;   // same as parameters[1];

            var result = $""{{num1}} + {{num2}} = {{(num1 + num2)}}"";

            Console.WriteLine(result);  // just for kicks in a test

            return result;
            ";

                        // should return a string: "10 + 20 = 30"
                        string results = exec.ExecuteCode<string>(code, 10, 20);

                        if (exec.Error)
                        {
                            Console.WriteLine($"Error: {exec.ErrorMessage}");
                            Console.WriteLine(exec.GeneratedClassCodeWithLineNumbers);

                        }*/

            /*Type type = typeof(arg);
            MethodInfo method = type.GetMethod(functionName, BindingFlags.Public | BindingFlags.Static);
            method.Invoke(null, null); // Static methods, with no parameters*/
            /* string code = @"
                     using System;

                     namespace UserFunctions
                     {                
                         public class BinaryFunction
                         {                
                             public static double Function(double x, double y)
                             {
                                 return func_xy;
                             }
                         }
                     }
                 ";
             string finalCode = code.Replace("func_xy", arg);
             CSharpCodeProvider provider = new CSharpCodeProvider();
             CompilerParameters parameters = new CompilerParameters();
             // Reference to System.Drawing library
             parameters.ReferencedAssemblies.Add("System.dll");
             // True - memory generation, false - external file generation
             parameters.GenerateInMemory = true;
             // True - exe file generation, false - dll file generation
             parameters.GenerateExecutable = true;
             CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);
             if (results.Errors.HasErrors)
             {
                 StringBuilder sb = new StringBuilder();

                 foreach (CompilerError error in results.Errors)
                 {
                     sb.AppendLine(string.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                 }

                 throw new InvalidOperationException(sb.ToString());
             }

             Assembly assembly = results.CompiledAssembly;
             Type program = assembly.GetType("First.Program");
             MethodInfo main = program.GetMethod("Main");

             main.Invoke(null, null);

             Type binaryFunction = results.CompiledAssembly.GetType("UserFunctions.BinaryFunction");
             return  binaryFunction.GetMethod("run");*/

       }

        private void CoreWebView2_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(e.WebMessageAsJson);
            Debug.WriteLine(json?["inputtype"]);
            if (json?["inputtype"] as String == "ai")
            {
                ApiCall(json?["data"]);
            }else if(json?["inputtype"] as String == "editor")
            {
               EditorProcess(json);

            }

            /*var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.meshy.ai");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "msy_Mbg1e6g4Ywe9mt5cWbs1nuw4zKwgwFCgXhDG");
            client.DefaultRequestHeaders
            .Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri("https://api.meshy.ai/v2/text-to-3d"));
            request.Content = new StringContent(e.WebMessageAsJson, Encoding.UTF8, "application/json");
            
           *//* await client.SendAsync(request).ContinueWith(responseTask =>
            {
                Debug.WriteLine("Response: {0}", responseTask.Result);
                
            });*//*
            var response = await client.PostAsync(new Uri("https://api.meshy.ai/v2/text-to-3d"), request.Content);
            var contents = await response.Content.ReadAsStringAsync();

            Debug.WriteLine(contents);
            var json = JsonConvert.DeserializeObject<Dictionary<string, string>>(contents);*/



            /*
                        var requestContent = new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string, string>("text","this is a block of text"),
                        });

                        HttpResponseMessage response = await client.PostAsync("", requestContent);

                        HttpContent responseContent = response.Content;

                        using(var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
                        {
                            Console.WriteLine(await reader.ReadToEndAsync());
                        }*/
            /*Debug.WriteLine(e.WebMessageAsJson);
            Debug.WriteLine(e.AdditionalObjects);*/
        }

        private async void CoreWebView2_WebResourceResponseReceived(object? sender, CoreWebView2WebResourceResponseReceivedEventArgs e)
        {
            // Actual headers sent with request
            foreach (var current in e.Request.Headers)
            {
                Console.WriteLine(current);
            }

            // Headers in response received
            foreach (var current in e.Response.Headers)
            {
                Console.WriteLine(current);
            }

            // Status code from response received
            int status = e.Response.StatusCode;
            if (status == 200)
            {
                Console.WriteLine("Request succeeded!");

                // Get response body
                try
                {
                    System.IO.Stream content = await e.Response.GetContentAsync();
                    // Null will be returned if no content was found for the response.
                    if (content != null)
                    {
                        /*DoSomethingWithResponseContent(content);*/


                    }
                }
                catch (COMException ex)
                {
                    // A COMException will be thrown if the content failed to load.
                }
            }
        }
    }
    
}