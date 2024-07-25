import * as monaco from 'https://esm.sh/monaco-editor';
import editorWorker from 'https://esm.sh/monaco-editor/esm/vs/editor/editor.worker?worker';
import jsonWorker from 'https://esm.sh/monaco-editor/esm/vs/language/json/json.worker?worker';
import cssWorker from 'https://esm.sh/monaco-editor/esm/vs/language/css/css.worker?worker';
import htmlWorker from 'https://esm.sh/monaco-editor/esm/vs/language/html/html.worker?worker';
import tsWorker from 'https://esm.sh/monaco-editor/esm/vs/language/typescript/ts.worker?worker';

class Editor {
	constructor(container,button) {
        this.cont = container;
        this.run = button;
		
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
            language: 'csharp'
        });
        console.log(out.getValue());
        this.run.addEventListener("click", () => {
            var j = {
                inputtype: "editor",
                data: out.getValue()
            }
            window.chrome.webview.postMessage(j);
            console.log("asldjfhkjhasdf");
        })
        //out
        window.chrome.webview.addEventListener("message", (ev) => {
            console.log(ev.data);
            var main = document.getElementById("maincode");
            var s = document.createElement("script");
            var sty = ev.data;
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
            console.log( String(ev.data).replace("\"", "").replace("\"", ""));
            s.innerHTML = String(ev.data);
            "window.BuyFruit=function(obj)"+
            "{"+
             "   return 1.00;"+
            "}"
            document.body.insertBefore(s, main);

            console.log(window.BuyFruit("sfs"));
          ///*  var f = new Function(ev.data);
          //  console.log(f);*/
        })
	}
}

export default Editor;