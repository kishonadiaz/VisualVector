import * as monaco from 'https://esm.sh/monaco-editor';
import editorWorker from 'https://esm.sh/monaco-editor/esm/vs/editor/editor.worker?worker';
import jsonWorker from 'https://esm.sh/monaco-editor/esm/vs/language/json/json.worker?worker';
import cssWorker from 'https://esm.sh/monaco-editor/esm/vs/language/css/css.worker?worker';
import htmlWorker from 'https://esm.sh/monaco-editor/esm/vs/language/html/html.worker?worker';
import tsWorker from 'https://esm.sh/monaco-editor/esm/vs/language/typescript/ts.worker?worker';
import * as THREE from 'three';
class Editor {
	constructor(container,button) {
        this.cont = container;
        this.run = button;
        window.editorEvent = new Event("editorEvent");
        this.Lcube = [];
;
        this.luuid = undefined;
        this.editorAction = [];
        this.methodsname = [];
        this.activemethods = {};
        this.atom = {};

;
	}
    action = () => {

    }
   
	editdisp = () => {
        self.MonacoEnvironment = {
            getWorker(_, label) {
                if (label === 'json') {
                    return new jsonWorker();
                }
                if (label === 'css' || label === 'scss' || label === 'less') {
                    return new cssWorker();
                }
                if (label === 'html' || label === 'handlebars' || label === 'razor') {
                    return new htmlWorker();
                }
                if (label === 'typescript' || label === 'javascript') {
                    return new tsWorker();
                }
                return new editorWorker();
            }
        };
        

        var out  = monaco.editor.create(document.getElementById(this.cont), {
            language: 'csharp',
            automaticLayout: true
        });
        console.log(out.getValue());
        this.run.addEventListener("click", () => {
            var sname = document.getElementById("scriptname").value;
            if (sname == "") {
                sname = "sd";
            }
            var j = {
                inputtype: "editor",
                scriptname: sname,
                data: out.getValue()
            }
            window.chrome.webview.postMessage(j);
            console.log("asldjfhkjhasdf");
        })
        //out
        var indexcount =0
        window.chrome.webview.addEventListener("message", (ev) => {

  
            console.log(ev.data, ev.scriptname, ev);
            var annotation = JSON.parse(ev.data.annotation);
            this.editorAction =  annotation.editoraction;
            console.log(annotation);
            this.atom = annotation;
            var main = document.getElementById("maincode");
            var s = document.getElementById("sd");
           /* if (s) {
                *//*document.body.removeChild(s);*//*
               
                //console.log(window["cube"]);
                setTimeout(() => {
                *//*    s.remove()
                    s.id = "sd";
                    s.type = "module";
                    s.innerHTML = "import * as THREE from 'three';";
                    s.innerHTML += String(ev.data.data);*//*
                    s.innerHTML = "import * as THREE from 'three';";
                    s.innerHTML += String(ev.data.data);
                    
                   *//* document.body.insertBefore(s, main);*//*
                }, 100)
               *//* alert("asdfhkjhfdsa");
                s.innerHTML = "import * as THREE from 'three';";
                s.innerHTML += String(ev.data.data);
                document.body.removeChild(s);
                s.src = "#?v=" + Math.random();
                s.remove()
                setTimeout(() => {
                    document.body.insertBefore(s, main);
                },200)*//*
                //
            }else {*/
            
            if (document.querySelector("#"+ev.data.scriptname + (indexcount - 1))) {
                document.querySelector("#"+ev.data.scriptname + (indexcount - 1)).remove();
            }
            var ed;
            window.Editor = this;
            if (this.Lcube) {
                console.log(this.methodsname);
                for (var i of Object.entries(this.Lcube)) {
                    for (var j of Object.entries(i[1])) {
                        console.log(j);
                    }
                }
            }
            
            var s = document.createElement("script");
            s.id = ev.data.scriptname+indexcount;
            s.type = "module";
            s.innerHTML = "import * as THREE from 'three';\n";
            s.innerHTML += String(ev.data.data);
            document.body.insertBefore(s, main);

            
            setTimeout(() => {
                //this.Lcube = window["cube"];
                /*console.log(this.Lcube, this.luuid);*/
                console.log(Object.entries(annotation["methodsname"]).length, this.methodsname.length, "here is a thing");
              /*  if (Object.entries(this.activemethods).length <= 0 || this.methodsname.length <= 0) {

                    for (var [iter, methodname] of Object.entries(annotation["methodsname"])) {
                        if (!this.methodsname.includes(methodname))
                            this.methodsname.push(String(methodname).trim());
                    }

                    for (var method of this.methodsname) {
                        this.activemethods[method] = window[method];
                    }

                } else {
                    for (var method of this.methodsname) {
                        this.activemethods[method] = window[method];
                    }
                    console.log(this.activemethods,window);
                   *//* for (var [iter, methodname] of Object.entries(annotation["methodsname"])) {
                        if (!this.methodsname.includes(methodname))
                            this.methodsname.push(String(methodname).trim());
                    }

                    for (var method of this.methodsname) {
                        this.activemethods[method] = window[method];
                    }*//*
                    for (var method of this.methodsname) {
                        window[method] = this.activemethods[method];
                    }
                    for (var method of this.methodsname) {
                       
                        *//*if (this.activemethods[method]["uuid"]) {
                            if (this.activemethods[method]["uuid"]) {
                                console.log(String(window[method]).length, annotation ,String(this.activemethods[method]).length)
                            }
                            
                        }*//*
                        *//*if (window[method] != this.activemethods[method]) {
                            window[method] = this.activemethods[method];
                        }*//*
                    }
                }*/
               /* if (this.methodsname.length > 0) {
                    for (var method of this.methodsname) {
                        if (window[method] != null || window[method] != undefined) {
                            //this.activemethods[method] = window[method];
                            //window[method] = this.activemethods[method];
                           
                            
                        }
                    }
                }*/

            },1)
            
            indexcount++;
            //}
           /* var sty = ev.data.data;
            while (sty.includes("\"")) {
                sty = sty.replace("\"", "");
            }
            var ss = sty;
            var srr = [];
            while (ss.includes("=")) {
                srr.push(ss.substr(ss.indexOf(".") + 1, Math.abs(ss.indexOf("="))).split("=")[0]);
                ss = ss.replace("=", "");
            }
            
            console.log(sty.indexOf(".") < sty.indexOf("="), sty.substr(sty.indexOf(".") + 1, Math.abs(sty.indexOf("="))).split("="), srr);
            console.log( String(ev.data.data).replace("\"", "").replace("\"", ""));
            
            "window.BuyFruit=function(obj)"+
            "{"+
             "   return 1.00;"+
            "}"
            */
            window.dispatchEvent(window.editorEvent);
           
            
          ///*  var f = new Function(ev.data);
          //  console.log(f);*/
        })
	}
}

export default Editor;