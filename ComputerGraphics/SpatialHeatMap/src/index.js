import * as THREE from 'three'
import axios from 'axios';

var scene = new THREE.Scene();
var camera = new THREE.PerspectiveCamera(75, window.innerWidth/window.innerHeight, 0.1, 1000);

camera.position.set(0, 0, 3);
camera.lookAt(new THREE.Vector3(0, 0, 0));
scene.add(camera);

var ambient = new THREE.AmbientLight( 0xffffff );
scene.add(ambient);

var light = new THREE.SpotLight( 0xffffff );
//scene.add(light);
//scene.add(light.target);

var renderer = new THREE.WebGLRenderer();
renderer.setSize(window.innerWidth, window.innerHeight);
document.body.appendChild(renderer.domElement);


function colormap(x){
    let r=0,g=0,b=0;
    if(x<0.5){
        let t=x/0.5;
        g=parseInt(t*255);
        b=parseInt((1-t)*255);
        console.log("A")
    }else{
        let t=(x-0.5)/0.5;
        r=parseInt(t*255);
        g=parseInt((1-t)*255)
        console.log("B")
    }
    let color = new THREE.Color('rgb(' + r + ',' + g + ',' + b + ')');
    return color;
} 

function init(classmap,fsz){
    let size=fsz;
    let halfsize=parseInt(size/2)
    window.cl = classmap
    // Definition 1
    for(let i=0;i<size;i++){
        for(let j=0;j<size;j++){
            for(let k=0;k<size;k++){
                let geometry1 = new THREE.BoxGeometry(0.6,0.6);
                let color =colormap(classmap[i][j][k])
                let material1 = new THREE.MeshLambertMaterial({color:color, transparent: true, opacity: 0.2});
                let box = new THREE.Mesh(geometry1, material1);
                box.translateX(i-halfsize);
                box.translateY(j-halfsize);
                box.translateZ(k-halfsize);
                scene.add(box);
            }
        }
    }
    setInterval(rot,100)
}

var time = 0
function rot(){
    time +=0.02;
    light.position.set(camera.x,camera.y,camera.z)
    light.castShadow= true;
    
    camera.lookAt(0,0,0)
    camera.position.x =32*Math.sin(time)
    camera.position.z = 32*Math.cos(time)
    camera.position.y =0
    renderer.render(scene, camera);
}

axios.get("data.json").then(function(response){
    window.cl=response.data.data
    console.log(response.data)
    init(response.data.data,16)
}).catch(function(exception){
    console.log(exception)
})