import os
os.environ["CUDA_VISIBLE_DEVICES"]="-1"

import tensorflow as tf
import numpy as np
import tensorflow.keras.backend as keras_backend
import matplotlib.pyplot as plt
import cv2
tf.compat.v1.disable_eager_execution()
tf.compat.v1.disable_v2_behavior()

mnist=tf.keras.datasets.mnist
(x_train,y_train),(x_test,y_test)=mnist.load_data()
x_test = np.expand_dims(tf.keras.utils.normalize(x_test,axis=1),axis=3)
x_train = np.expand_dims(tf.keras.utils.normalize(x_train,axis=1),axis=3)

conv1 = tf.keras.layers.Conv2D(8,(5,5))
actv1 = tf.keras.layers.ReLU()
pool1 = tf.keras.layers.MaxPool2D()
conv2 = tf.keras.layers.Conv2D(64,(5,5))
actv2 = tf.keras.layers.ReLU()
pool2 = tf.keras.layers.MaxPool2D()
flatten = tf.keras.layers.Flatten()
dense3 = tf.keras.layers.Dense(10,activation=tf.nn.softmax)

input = tf.keras.layers.Input((28,28,1))
x=input
x = conv1(x)
x = actv1(x)
x = pool1(x)
#x = conv2(x)
#x = actv2(x)
#x = pool2(x)
x = flatten(x)
x = dense3(x)

model = tf.keras.Model(inputs=[input],outputs=[x])
model.set_weights(tf.keras.models.load_model("model").get_weights())
model.summary()

last_conv = model.get_layer('conv2d')
sample = x_train[np.random.randint(0,x_train.shape[0])]
input = np.expand_dims(sample,axis=0)
sample_predict = model.predict(input)


matched_index= np.argmax(sample_predict)
matched_model_output = model.output[:,matched_index]

grads = keras_backend.gradients(matched_model_output,last_conv.output)[0]
pooled_grads = keras_backend.mean(grads, axis=(0, 1, 2))
iterate = keras_backend.function([model.input], [pooled_grads, last_conv.output[0]])
pooled_grads_value, conv_layer_output_value = iterate([input])

for i in range(conv_layer_output_value.shape[2]):
    conv_layer_output_value[:, :, i] *= pooled_grads_value[i]

heatmap = np.mean(conv_layer_output_value, axis=-1)
heatmap = np.maximum(heatmap, 0)
heatmap /= np.max(heatmap)

rgb_img = np.concatenate((sample,sample,sample),axis=2)*255
rgb_img = rgb_img.astype("int")
heatmap = np.uint8(cv2.resize(heatmap,(rgb_img.shape[1],rgb_img.shape[0]))*255)
heatmap = cv2.applyColorMap(heatmap, cv2.COLORMAP_JET)
imagnew = np.uint8(heatmap*0.5 + rgb_img)
imagnewrgb = imagnew[...,::-1]

plt.imshow(imagnewrgb)
plt.show()