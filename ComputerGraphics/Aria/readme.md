## Aria

Visualize some stuffs using native WebGL.



### Main Dependencies

Nodejs, Webpack, Axios, TypeScript(ts-loader)，gl-matrix-ts, webgl-gltf



### How to Run

Execute the command `npm run deploy` to establish the debug server.

Execute the command `npm run build` to build the production bundle.



### Available Stages

Open the browser with url param `stage` to choose stages

Available stages are `gltf`, `fog`, `value_noise`, `perlin_noise`, `volume_render`, `fxaa`, `gerstner_wave`, `vol_light`, `ssao`, `ssr`

For example: `http://localhost:1551/?stage=volume_render`



### License

The license coincides with the repository to which this project belongs



### Material Referenced / Acknowledgements

**Wavefront African Model (Incl. Specular, Diffuse, Normal Texture) ** : By Dmitry V. Sokolov. In Project "TinyRenderer" https://github.com/ssloy/tinyrenderer

**Bonzai Volume Texture**: By Leandro Barbagallo. In Project "WebGLVolumeRendering" https://github.com/lebarba/WebGLVolumeRendering/

**Skybox Texture**: By Joey de Vries. In Project "LearnOpenGL" https://learnopengl.com/

**Wood PBR Texture:** https://freepbr.com/wp-content/uploads/bl/bamboo-wood-semigloss-bl.zip

**Klee GLTF Model**: By INSTICT. In Creation "Klee FBX", Converted by Sktechfab, Re-exported using Blender. https://sketchfab.com/3d-models/klee-fbx-eb4ba3c0d92349cfaf7361e4e55604f0



### Tutorials Referenced / Acknowledgements

https://learnopengl.com/

https://github.com/lebarba/WebGLVolumeRendering/

https://zhuanlan.zhihu.com/p/431384101

https://zhuanlan.zhihu.com/p/404778222

http://www.alexandre-pestana.com/volumetric-lights/
