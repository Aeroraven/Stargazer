import os
os.environ["CUDA_VISIBLE_DEVICES"]="-1"

import tensorflow as tf
import numpy as np
import matplotlib.pyplot as plt
import cv2
# Shit!
# tf.compat.v1.disable_v2_behavior()

# Get Dataset
mnist=tf.keras.datasets.mnist
(x_train,y_train),(x_test,y_test)=mnist.load_data()
x_test = np.expand_dims(tf.keras.utils.normalize(x_test,axis=1),axis=3)
x_train = np.expand_dims(tf.keras.utils.normalize(x_train,axis=1),axis=3)

# Define Model
conv1 = tf.keras.layers.Conv2D(8,(5,5))
actv1 = tf.keras.layers.ReLU()
pool1 = tf.keras.layers.MaxPool2D()
conv2 = tf.keras.layers.Conv2D(64,(5,5))
actv2 = tf.keras.layers.ReLU()
pool2 = tf.keras.layers.MaxPool2D()
gap1 = tf.keras.layers.GlobalAvgPool2D()
flatten = tf.keras.layers.Flatten()
dense3 = tf.keras.layers.Dense(10,activation=tf.nn.softmax,use_bias=False)

input = tf.keras.layers.Input((28,28,1))
x=input
x = conv1(x)
x = actv1(x)
#x = pool1(x)
x = conv2(x)
x = actv2(x)
#x = pool2(x)
x = gap1(x)
x = dense3(x)

# Train
model = tf.keras.Model(inputs=[input],outputs=[x])
model.set_weights(tf.keras.models.load_model("model_cam").get_weights())

# Get Output
idx = np.random.randint(0,x_train.shape[0])
idx= 144
sample = x_train[idx]
inputx = np.expand_dims(sample,axis=0)
sample_predict = model.predict(inputx)
matched_index= np.argmax(sample_predict)
print(matched_index)

# Get Hidden Layers
last_conv_model = tf.keras.Model(inputs=[input],outputs=[model.get_layer('conv2d_1').output])
last_conv_output = last_conv_model.predict(inputx)
gap_layer = model.get_layer('global_average_pooling2d')
fc_layer = model.get_layer('dense')

# Calculate CAM
weights = fc_layer.weights[0]
maps = last_conv_output[0].transpose(2,0,1)
cam = []
for i in range(10):
    weighted_cam = np.zeros((maps.shape[1],maps.shape[2]))
    class_weight = weights.numpy()[:,i]
    for j in range(maps.shape[0]):
        conv_output = maps[j]
        conv_class_weight = class_weight[j]
        weighted_cam += conv_output*conv_class_weight
    cam.append(weighted_cam)
cam = np.array(cam)


# Heatmap
inputx = np.expand_dims(inputx.squeeze(),axis=2)
rgb_img = np.concatenate((inputx,inputx,inputx),axis=2)*255
rgb_img = rgb_img.astype("int")
heatmap = cam[matched_index]
heatmap2 = ((heatmap - np.min(heatmap))/(np.max(heatmap)-np.min(heatmap))*255).astype("uint8")
heatmap3 = cv2.resize(heatmap2.astype("uint8"),(28,28))
heatmap3 = cv2.applyColorMap(heatmap3, cv2.COLORMAP_JET)
imagnew = np.uint8(heatmap3*0.4+rgb_img*0.6)
plt.imshow(imagnew)
plt.show()

# 1156 - 7
# 72 - 1
# 8965 - 7
# 8848 - 6
# 23 - 1