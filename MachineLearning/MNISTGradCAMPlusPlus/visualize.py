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
idx = 2525
sample = x_train[idx]
sample_predict = model.predict(np.expand_dims(sample,axis=0),batch_size=1)
fc_model = keras.Model(inputs=[input],outputs=[model.get_layer('dense_2').output])
fc_output = fc_model.predict(np.expand_dims(sample,axis=0),batch_size=1)
fc_best = np.argmax(fc_output)

# Get last conv layer
last_conv_layer = fc_model.get_layer('conv2d_1')

# Get gradient
gradient_tensor_order1 = keras.backend.gradients(keras.backend.exp(fc_model.output[:,fc_best]),last_conv_layer.output)[0]
gradient_tensor_order2 = keras.backend.gradients(gradient_tensor_order1,last_conv_layer.output)[0]
gradient_tensor_order3 = keras.backend.gradients(gradient_tensor_order2,last_conv_layer.output)[0]

# Get the output of last conv layer
last_conv_calc_func = keras.backend.function([fc_model.input],[last_conv_layer.output[0]])
conv_output = last_conv_calc_func([np.expand_dims(sample,axis=0)])[0]

# Calculate alpha
feature = conv_output.shape[2]
feature_map_sum = np.zeros((feature))
for i in range(feature):
    for a in range(conv_output.shape[0]):
        for b in range(conv_output.shape[1]):
            feature_map_sum[i] += conv_output[a,b,i]

# Calculate gradients
grad_func = keras.backend.function([fc_model.input],[gradient_tensor_order1,gradient_tensor_order2,gradient_tensor_order3])
grad_1,grad_2,grad_3 = grad_func([np.expand_dims(sample,axis=0)])
grad_1 = grad_1.squeeze()
grad_2 = grad_2.squeeze()
grad_3 = grad_3.squeeze()

# Define ReLU
def relu(x):
    return np.maximum(x,0)

# Calculate Weights
weight = np.zeros((feature))
for k in range(feature):
    for i in range(conv_output.shape[0]):
        for j in range(conv_output.shape[1]):
            if np.abs((2*grad_2[i,j,k]+feature_map_sum[k]*grad_3[i,j,k])) > np.finfo('float').eps:
                weight[k]+=(grad_2[i,j,k])/(2*grad_2[i,j,k]+feature_map_sum[k]*grad_3[i,j,k])*relu(grad_1[i,j,k])

# Calculate GradCAM
grad_cam = np.zeros((conv_output.shape[0],conv_output.shape[1])).astype("float32")
feature = conv_output.shape[2]
conv_output = conv_output.transpose(2,0,1)
for i in range(feature):
    grad_cam = grad_cam + weight[i]*conv_output[i]

# Apply heatmap
inputx = np.expand_dims(sample.squeeze(), axis=2)
rgb_img = np.concatenate((inputx, inputx, inputx), axis=2) * 255
rgb_img = rgb_img.astype("int")
heatmap = -grad_cam
heatmap2 = ((heatmap - np.min(heatmap)) / (np.max(heatmap) - np.min(heatmap)) * 255).astype("uint8")
heatmap3 = cv2.resize(heatmap2.astype("uint8"), (28, 28))
heatmap3 = cv2.applyColorMap(heatmap3, cv2.COLORMAP_JET)
mixed = heatmap3 * 0.4 + rgb_img * 0.6
imagnew = np.uint8((mixed) * 205 / np.max(mixed) + 50)

# Visualize Grad-CAM Plus Plus
plt.imshow(imagnew)
plt.show()