import * as THREE from 'three';

class Stage{
    constructor(scene,clock,camera,renderer) {
        this.scene = scene;
        this.renderer = renderer;
        this.camera = camera;
        this.stagedobj = [];
        this.clock = clock;
        this.theta = 0;
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

        this.render();
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
        this.animate();
    }

}

export default Stage;