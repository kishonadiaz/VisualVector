import * as THREE from 'three';

class Stage{
    constructor(scene,clock,camera,editor,renderer) {
        this.scene = scene;
        this.renderer = renderer;
        this.camera = camera;
        this.stagedobj = [];
        this.clock = clock;
        this.theta = 0;
        this.editor = editor;
        this.count = 0;
        this.animationfunctions = [];
        this.editobjactions = {}; 
        
    }
    stagebgcolor = (color) => {
        this.scene.background = new THREE.Color(color);

    }
    get = (val) =>{
        return this[val];
    }
    add = (item) => {
        this.stagedobj.push(item);
    }
    
    animate = () => {
        requestAnimationFrame(this.animate);
        const delta = this.clock.getDelta();
       /* if (Object.entries(this.editor.Lcube).length > 0)
            console.log(this.editor.Lcube);*/
        //console.log(window,this.editor.Lcube["cube"]);
        /*for (var [i, item] of Object.entries(this.editor.Lcube)) {
            console.log(window[i]);
            //window[i] = item;
            if (window[i] != item) {
                window[i] = item;
                *//*console.log(window[i]);*//*
            }
        }*/
        //console.log(this.editor.Lcube);
        /*if (this.editor.Lcube.length > 0)
            cubeanimate(this.count, this.editor);*/
        /*if (Object.entries(this.editor.Lcube).length > 0)
            window["cube"] = this.editor.Lcube["cube"];*/

       // console.log(this.animationfunctions);

        if (this.animationfunctions.length > 0) {
            for (var i of this.animationfunctions) {
                //console.log(cube, this.editor.Lcube);
                if (this.editor.Lcube.length > 0) {
                    if (cube != this.editor.Lcube[0]) {
                        cube = this.editor.Lcube[0];
                    }
                }
                

                i["animate"](delta, cube);
                //i(delta, this.editor.Lcube);
                
            }
        }

       /* if (this.editor.methodsname.length > 0) {
            for (var method of this.editor.methodsname) {
                if (window[method] != null || window[method] != undefined) {
                    //window[method] = this.editor.activemethods[method];
                    if (typeof window[method] != "object" && typeof window[method] != "function" ) {
                        window[method] = this.editor.activemethods[method];
                    }
                    if (typeof window[method] == "function") {
                        window[method](delta, this.editor.Lcube);
                        console.log(window[method])
                    } else {
                        if (typeof window[method] != "object") {
                            window[method] = this.editor.activemethods[method];
                        } else {
                            window[method] = this.editor.activemethods[method];
                        }
                    }
                }
                    

            }
        }
*/
        this.render();
        this.count += 1;
  
    }
    resize = () => {
        this.camera.aspect = window.innerWidth / window.innerHeight
        this.camera.updateProjectionMatrix()
        this.renderer.setSize(window.innerWidth, window.innerHeight)
    }
    render = () => {
        this.theta += 0.1;
        this.renderer.render(this.scene, this.camera);
    }
    mainstage = () => {
        for (var item of this.stagedobj) {
            
            if (typeof item == "function") {
                item();
            } else {
                console.log(this.scene);
                this.scene.add(item);
            }
        }
        addEventListener("resize", this.resize);
        var childed = undefined;
        addEventListener("editorEvent", () => {
            window.Renderer = this.renderer;
            window.Scene = this.scene;
            window.Camera = this.camera;
            this.arr = [];

            setTimeout(() => {

                
               console.log(window, this.editor.editorAction);
                if (Object.entries(this.editor.editorAction).length > 0) {
                    for (var [i, item] of Object.entries(this.editor.editorAction)) {
                        console.log(i, item);
                        if (i.includes("animate")) {
                            console.log("ooooooo");
                            var name = item
                            var obj = {};
                            obj["animate"] = new Function("delta", "scenobjs", item+"(delta,scenobjs)")
                            this.animationfunctions.push(obj);
                        }
                    }
                }
                this.scene.traverse((child) => {
                    var l = []
                   
                    /*console.log(child.uuid, this.editor.Lcube.includes(child));*/
                    if (this.editor.Lcube.includes(child)) {
                       // alert("afljkhasfd");
                        this.editor.luuid = child.uuid;
                        console.log(child.uuid, this.editor.Lcube, this.editor.Lcube.find((childs) => {
                            return childs;
                        }));
                        child = this.editor.Lcube.find((childs) => {
                            return childs;
                        });
                        //child = window.cube //apply same material to all meshes
                        //this.editor.lcube = child;
                        
                    } else {
                        
                        
                    }
                });
                console.log(window);
                console.log(this.editor.Lcube, this.editor.luuid);
                //Working here so
              /* 
                if (this.editor.Lcube.length > 0) {
                    //alert("okkk");
                    if (this.editor.Lcube.uuid === this.editor.luuid) {
                        //alert("ok");
                        //window.cube = this.editor.Lcube;
                    } 
                    window.cube = this.editor.Lcube[0];
                   
                } else {
                    this.scene.add(window.cube);
                    this.editor.Lcube.push(window.cube);

                }*/
                console.log(this.editor.methodsname);
                for (var methodname of this.editor.methodsname) {
                    console.log(window[methodname.trim()]);
                    if (this.editor.Lcube.length <= 0) {
                        console.log(window[methodname.trim()] instanceof THREE.Object3D);
                        if ((window[methodname.trim()] instanceof THREE.Object3D) === true) {
                            if (typeof window[methodname.trim()] != "function" && typeof window[methodname.trim()] != "array") {
                                console.log(window[methodname.trim().toString()], methodname.trim(), this.editor.Lcube);
                                if (window[methodname.trim().toString()].length);
                                else {
                                    //this.scene.add((window[methodname.trim().toString()]));
                                    this.editor.Lcube.push(window[methodname.trim()]);
                                    this.editobjactions[methodname.trim()] = window[methodname.trim()];
                                }
                                //
                            }

                        } 
                        
                    } else {
                        for (var [i,its] of Object.entries(this.editor.Lcube)) {
                            console.log(i, its);
                            if (window[methodname.trim()] && this.editor.Lcube[i] == window[methodname.trim()]) {

                                this.editor.Lcube[i] = window[methodname.trim()];
                               
                            } else {
                                if (methodname.length != this.editor.Lcube.length) {
                                    if ((window[methodname.trim()] instanceof THREE.Object3D) === true) {
                                        if (!this.editor.Lcube.includes(window[methodname.trim()])) {
                                            if (typeof window[methodname.trim()] != "function" && typeof window[methodname.trim()] != "array") {
                                                if (this.editobjactions[methodname.trim()] != window[methodname.trim()]) {
                                                    if (this.editor.Lcube[i - 1])
                                                    if (Object.hasOwn(this.editor.Lcube[i-1],"uuid")) {
                                                        if (this.editor.Lcube[i]["uuid"] != this.editor.Lcube[i - 1]["uuid"]) {
                                                            this.editor.Lcube.push(window[methodname.trim()]);
                                                            this.editobjactions[methodname.trim()] = window[methodname.trim()];
                                                        }
                                                    }
                                                } else {
                                                    console.log("asfldjhkasfdsss");
                                                }
                                               /* if (this.editor.Lcube[i] != window[methodname.trim()]) {
                                                    //this.scene.add(window[methodname.trim()]);
                                                    this.editor.Lcube.push(window[methodname.trim()]);
                                                }*/
                                                /*if (window[methodname.trim().toString()].length);
                                                else {
                                                    if (this.editor.Lcube.includes(window[methodname.trim()] != this.editor.Lcube[i - 1])) {
                                                        this.scene.add(window[methodname.trim()]);
                                                        this.editor.Lcube.push(window[methodname.trim()]);
                                                    } else {

                                                    }

                                                }*/

                                            }
                                        }
                                    }
                                }
                                /*if ((window[methodname.trim()] instanceof THREE.Object3D) === true) {
                                    if (!this.editor.Lcube.includes(window[methodname.trim()])) {
                                        if (typeof window[methodname.trim()] != "function" && typeof window[methodname.trim()] != "array") {
                                            if (window[methodname.trim().toString()].length);
                                            else {
                                                if (this.editor.Lcube.includes(window[methodname.trim()] != this.editor.Lcube[i - 1])){
                                                    this.scene.add(window[methodname.trim()]);
                                                    this.editor.Lcube.push(window[methodname.trim()]);
                                                } else {
                                                    
                                                }
                                               
                                            }

                                        }
                                    }
                                }*/
                                
                            }
                        }
                    }

                    for (var [j, ir] of Object.entries(this.editobjactions)) {
                        if (j.includes(methodname.trim())) {
                            this.scene.add((window[j]));
                        }
                    }
                }
                console.log(this.editor.atom);
              /*  for (var methodname of this.editor.methodsname) {
                    if (window[methodname]) {
                       
                        console.log(window[methodname], methodname);
                        this.arr.push(window[methodname]);
                        if (!Object.hasOwn(this.editor.Lcube, methodname)) {
                            
                            this.editor.Lcube[methodname] = window[methodname];
                            //this.arr.push(window[methodname])
                        } else {
                            console.log(Object.entries(this.editor.Lcube), window[methodname]);
                            
                            for (var [i, item] of Object.entries(this.editor.Lcube)) {
                                console.log(i, item, this.arr);
                                //this.editor.Lcube[methodname] = window[methodname];
                                
                                *//*if (Object.hasOwn(this.editor.Lcube[i + 1], 'position')) {
                                    if (Object.hasOwn(this.editor.Lcube[i + 1].position, "copy")) {
                                        this.editor.Lcube[i].position.copy(this.editor.Lcube[i + 1].position);
                                        this.editobjactions[methodname] = this.editor.Lcube[i];
                                        this.editor.Lcube.pop();

                                    }
                                }*//*
                            }
                        }
                       *//* if (!this.editor.Lcube.includes(window[methodname]))
                            this.editor.Lcube.push(window[methodname]);
                        else {
                            for (var i = 0; i < this.editor.Lcube.length; i++) {
                                if (this.editor.Lcube.length > Object.entries(this.editor.methodsname).length) {
                                    console.log(i, "dlsjkaasdfjkhajksf");
                                }
                            }
                        }
                        
                        for (var i = 0; i < this.editor.Lcube.length; i++) {

                            if (this.editor.Lcube[i + 1]) {
                                // alert("asdfljh");
                                console.log(this.editor.Lcube[i])
                                if (Object.hasOwn(this.editor.Lcube[i + 1], 'position')) {
                                    if (Object.hasOwn(this.editor.Lcube[i + 1].position, "copy")){
                                        this.editor.Lcube[i].position.copy(this.editor.Lcube[i+1].position);
                                        this.editobjactions[methodname] =this.editor.Lcube[i];
                                        this.editor.Lcube.pop();

                                    }
                                }
                                
                                

                            }
                        }*//*
                    }
                }*/
               
                
                if (!this.editor.Lcube.length <= 0) {
                    console.log("aslfdkjlasdfhklahfsd");
                    this.editor.Lcube.push(window["cube"]);
                }
                else {
                    for (var i = 0; i < this.editor.Lcube.length; i++) {

                        if (this.editor.Lcube[i + 1]) {
                            // alert("asdfljh");
                            console.log(this.editor.Lcube[i])
                            this.editor.Lcube[0].position.copy(this.editor.Lcube[i + 1].position);
                            window[methodname] = this.editor.Lcube[0];
                            this.editor.Lcube.pop();
                            /* if (Object.hasOwn(this.editor.Lcube[i + 1], 'position')) {
                                 if (Object.hasOwn(this.editor.Lcube[i + 1].position, "copy")) {
                                     
    
                                 }
                             }*/



                        }
                    }
                }
                       
                
                
                
                setTimeout(() => {
                    /*console.log(window);
                    console.log(this.editor.Lcube, this.editor.luuid);
                    if (this.editor.Lcube) {
                        alert("okkk");
                        if (this.editor.Lcube.uuid == this.editor.luuid) {
                            alert("ok");
                            childed = window.cube;
                        }
                    } else {
                        this.scene.add(window.cube);
                    }
                    this.render();*/
                },10)
                
                console.log(window.cube);
                for (var i of this.scene.children) {
                    console.log(i);
                }
                
                
                console.log(this.scene);
                
            }, 200)
        })
       /* setTimeout(() => {
            console.log(window, window.BuyFruit("ljashdfjk"));
            this.scene.add(window.cube);
        }, 200)*/
        this.animate();
    }

}

export default Stage;