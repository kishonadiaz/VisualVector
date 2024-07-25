import * as THREE from 'three';
import WebGL from 'three/addons/capabilities/WebGL.js';
import { OrbitControls } from 'three/addons/controls/OrbitControls.js';
import Stage from './Componets/stage.js';
import Editor from './Componets/editor.js';
import * as monaco from 'https://esm.sh/monaco-editor';
import editorWorker from 'https://esm.sh/monaco-editor/esm/vs/editor/editor.worker?worker';
import jsonWorker from 'https://esm.sh/monaco-editor/esm/vs/language/json/json.worker?worker';
import cssWorker from 'https://esm.sh/monaco-editor/esm/vs/language/css/css.worker?worker';
import htmlWorker from 'https://esm.sh/monaco-editor/esm/vs/language/html/html.worker?worker';
import tsWorker from 'https://esm.sh/monaco-editor/esm/vs/language/typescript/ts.worker?worker';

var editor = new Editor("editorcont",document.getElementById("run"));

editor.editdisp();

var canvas = document.querySelector("#maincanvas");
canvas.addEventListener

const scene = new THREE.Scene();
const camera = new THREE.PerspectiveCamera(60, window.innerWidth / window.innerHeight, 1.0, 30000);
camera.position.set(75, 20, 0);

let clock, renderer, raycaster;
let INTERSECTED;
let theta = 0;

clock = new THREE.Clock();

const AmbientLightf = (color, intensity) => {
    const al = new THREE.AmbientLight(color, intensity);
    return al;
}
const HemisphereLights = (gcolor, scolor, intensity, callback = () => { }) => {
    const hemiLight = new THREE.HemisphereLight(gcolor, scolor, intensity);
    callback(hemiLight);
    return hemiLight;
}
const DirectionalLightf = (color, intensity, callback = () => { }) => {
    const dirLight = new THREE.DirectionalLight(color, intensity);
    callback(dirLight);
    return dirLight;

}

if (WebGL.isWebGLAvailable()) {
    var maincont = document.getElementById("maincont")
    renderer = new THREE.WebGLRenderer({ antialias: true, canvas: canvas });
    renderer.setSize(window.innerWidth, window.innerHeight);
    renderer.shadowMap.enabled = true;
    maincont.appendChild(renderer.domElement);
    var stage = new Stage(scene, clock, camera, renderer);
    stage.stagebgcolor(0xffffff);
    const mesh = new THREE.Mesh(new THREE.PlaneGeometry(1000, 1000, 1, 1), new THREE.MeshPhongMaterial({ color: 0xcbcbcb }));
    mesh.rotation.x = - Math.PI / 2;
    mesh.receiveShadow = true;
    mesh.position.set(0, 0, 0);
    stage.add(mesh);

    const geometry = new THREE.BoxGeometry(100, 100, 100);
    let materialArrays = [];
    materialArrays.push(new THREE.MeshPhongMaterial({ color: 0x00ff00 }))
    materialArrays.push(new THREE.MeshPhongMaterial({ color: new THREE.Color("Red") }))
    materialArrays.push(new THREE.MeshPhongMaterial({ color: 0xffffff }))
    materialArrays.push(new THREE.MeshPhongMaterial({ color: 0x32f888 }))
    materialArrays.push(new THREE.MeshPhongMaterial({ color: 0x00ff00 }))
    materialArrays.push(new THREE.MeshPhongMaterial({ color: 0x00ff00 }))
    var cube = new THREE.Mesh(geometry, materialArrays);
    cube.position.set(0, 55, 0);
    stage.add(cube);
    stage.add(AmbientLightf(0xffffff,1));
    stage.add(HemisphereLights(0xfffff,0xfffff,9));
    stage.add(DirectionalLightf(0xffffff, 1, (dirLight) => {
        dirLight.position.set(- 3, 10, - 10);
        dirLight.castShadow = true;
        dirLight.shadow.camera.top = 2;
        dirLight.shadow.camera.bottom = - 2;
        dirLight.shadow.camera.left = - 2;
        dirLight.shadow.camera.right = 2;
        dirLight.shadow.camera.near = 0.1;
        dirLight.shadow.camera.far = 40;
    }));

    stage.mainstage();
    const controls = new OrbitControls(camera, renderer.domElement);
    controls.target.set(0, 0, 0);
    // controls.minDistance = 500;
    // controls.maxDistance = 1500;
    controls.update();


    controls.addEventListener('change', function () {

        

    });

} else {
    console.warn(WebGL.getWebGLErrorMessage());
}
var stage = new Stage();

