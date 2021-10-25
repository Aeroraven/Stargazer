import os
os.environ["CUDA_VISIBLE_DEVICES"]="-1"

import tensorflow.keras as keras
import tensorflow.compat.v1 as tf
import matplotlib.pyplot as plt
import numpy as np
import cv2


# Shit!
# tf.compat.v1.disable_eager_execution()
tf.disable_v2_behavior()

# Get Dataset
mnist=keras.datasets.mnist
(x_train,y_train),(x_test,y_test)=mnist.load_data()
x_test = np.expand_dims(keras.utils.normalize(x_test,axis=1),axis=3)
x_train = np.expand_dims(keras.utils.normalize(x_train,axis=1),axis=3)

# Define Model
conv1 = keras.layers.Conv2D(8,(3,3))
actv1 = keras.layers.ReLU()
pool1 = keras.layers.AvgPool2D()
conv2 = keras.layers.Conv2D(16,(5,5))
actv2 = keras.layers.ReLU()
pool2 = keras.layers.AvgPool2D()
flatten = keras.layers.Flatten()
dense1 = keras.layers.Dense(256)
dense2 = keras.layers.Dense(64)
dense3 = keras.layers.Dense(10)
softmax = keras.layers.Softmax()

input = keras.layers.Input((28,28,1))
x = conv1(input)
x = actv1(x)
x = conv2(x)
x = actv2(x)
x = pool2(x)
x = flatten(x)
x = dense1(x)
x = dense2(x)
x = dense3(x)
x = softmax(x)

# Load
model = keras.Model(inputs=[input],outputs=[x])
model.set_weights(keras.models.load_model('model_gradcam').get_weights())
model.summary()


# Get Classification Score
idx = 1527
sample = x_train[idx]
sample_predict = model.predict(np.expand_dims(sample,axis=0),batch_size=1)
fc_model = keras.Model(inputs=[input],outputs=[model.get_layer('dense_2').output])
fc_output = fc_model.predict(np.expand_dims(sample,axis=0),batch_size=1)
fc_best = np.argmax(fc_output)

# Get last conv layer
last_conv_layer = fc_model.get_layer('conv2d_1')

# Get gradient
gradient_tensor = keras.backend.gradients(fc_model.output[:,fc_best],last_conv_layer.output)[0]

# Apply Global-average-pooling on every feature map (gradient)
gap_gradient_tensor = keras.backend.mean(gradient_tensor,axis=(0,1,2))

# When forwarding, calculate gradients via replacing placeholder in gradient tensor(weight: a_c^k)
# and obtain the conv output(feature map: A_k)
calculate_function = keras.backend.function([fc_model.input],[gap_gradient_tensor,last_conv_layer.output[0]])
gap_gradient, conv_output = calculate_function([np.expand_dims(sample,axis=0)])

# Calculate the initial gradient-weighted class activation map
grad_cam = np.zeros((conv_output.shape[0],conv_output.shape[1])).astype("float32")
feature = conv_output.shape[2]
conv_output = conv_output.transpose(2,0,1)
for i in range(feature):
    grad_cam = grad_cam + gap_gradient[i]*conv_output[i]

# Apply ReLU activation
# grad_cam = np.maximum(grad_cam,0)

# Apply heatmap
inputx = np.expand_dims(sample.squeeze(),axis=2)
rgb_img = np.concatenate((inputx,inputx,inputx),axis=2)*255
rgb_img = rgb_img.astype("int")
heatmap = grad_cam
heatmap2 = ((heatmap - np.min(heatmap))/(np.max(heatmap)-np.min(heatmap))*255).astype("uint8")
heatmap3 = cv2.resize(heatmap2.astype("uint8"),(28,28))
heatmap3 = cv2.applyColorMap(heatmap3, cv2.COLORMAP_JET)
mixed = heatmap3*0.4+rgb_img*0.6
imagnew = np.uint8((mixed)*205/np.max(mixed)+50)

# Visualize Grad-CAM
plt.imshow(imagnew)
plt.show()