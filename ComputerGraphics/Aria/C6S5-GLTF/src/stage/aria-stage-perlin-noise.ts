import { AriaComBuffers } from "../components/core/aria-com-buffers";
import { AriaComMesh } from "../components/core/aria-com-mesh";
import { AriaComScene } from "../components/core/aria-com-mesh-composite";
import { AriaComTimestamp } from "../components/effects/aria-com-timestamp";
import { AriaComRect } from "../components/geometry/aria-com-rect";
import { AriaAssetLoader } from "../core/aria-asset-loader";
import { AriaCamera } from "../core/aria-camera";
import { AriaPageIndicator } from "../core/aria-page-indicator";
import { AriaStage } from "./aria-stage-base";

export class AriaStagePerlinNoise extends AriaStage{
    renderEnt:()=>void

    constructor(){
        super()
        this.renderEnt = ()=>{}
    }

    public async prepare(gl: WebGL2RenderingContext): Promise<any> {
        const camera = new AriaCamera()
        const assets = await AriaAssetLoader.getInstance(gl)

        AriaPageIndicator.getInstance().updateLoadingTip("Preparing")

        //Extensions
        const extCheck = (x:unknown)=>{return (x==null)};
        if(extCheck(gl.getExtension('EXT_color_buffer_float'))){
            alert("Floating buffer is not enabled / EXT_color_buffer_float")
        }

        //Viewport
        gl.viewport(0,0,window.innerWidth,window.innerHeight)
        gl.disable(gl.CULL_FACE)

        //Stencil Test
        gl.enable(gl.STENCIL_TEST)
        gl.stencilOp(gl.KEEP,gl.KEEP,gl.REPLACE)
        gl.stencilFunc(gl.ALWAYS,1,0xff)
        gl.stencilMask(0xff)

        //Camera
        camera.registerInteractionEvent()

        //Scene
        const mainScene = (<AriaComScene>AriaComScene.create(gl))

        //Animation Component
        const shaderTimer  = (<AriaComTimestamp>AriaComTimestamp.create(gl))

        //Mesh
        const postOrgRect = (<AriaComRect>AriaComRect.create(gl))
        const postOrgBuffer = (<AriaComBuffers>AriaComBuffers.create(gl))
            .addGeometry(postOrgRect)
        const postOrgMesh =   (<AriaComMesh>AriaComMesh.create(gl))
            .setCamera(camera)
            .setShader(assets.getShader("post-perlin-noise-3d"))
            .setBuffer(postOrgBuffer)
            .addAttachments("timer",shaderTimer)
        mainScene.addObject(postOrgMesh)

        AriaPageIndicator.getInstance().done()
        let last = Date.now()
        let turns = 0

        function clearScene(gl:WebGL2RenderingContext){ 
            gl.clearColor(1,1,1,1);
            gl.enable(gl.DEPTH_TEST);
            gl.depthFunc(gl.LEQUAL);
            gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT | gl.STENCIL_BUFFER_BIT);
        } 

        const renderCall = ()=>{
            turns++;
            if(turns%5==0){
                let fps = 1000/(Date.now()-last)*5;
                last = Date.now()
                AriaPageIndicator.getInstance().updateFPS(fps)
                turns = 0;
            }

            clearScene(gl)
            mainScene.render()
        }
        this.renderEnt = renderCall
    }

    public async render(gl: WebGL2RenderingContext): Promise<any> {
        this.renderEnt()
    }

}