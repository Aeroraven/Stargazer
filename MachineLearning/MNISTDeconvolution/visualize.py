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

# Load trained model
model = keras.Model(inputs=[input],outputs=[x])
model.set_weights(keras.models.load_model('model_deconv').get_weights())
model.summary()

# Deconvolution
unpool1 = keras.layers.UpSampling2D()
rev_actv1 = keras.layers.ReLU()
deconv1= keras.layers.Conv2DTranspose(8,(5,5),weights=[model.get_layer('conv2d_1').get_weights()[0],np.zeros(8).astype("float32")])
rev_actv2 = keras.layers.ReLU()
deconv2 = keras.layers.Conv2DTranspose(1,(3,3),weights=[model.get_layer('conv2d').get_weights()[0],np.zeros(1).astype("float32")])
y=unpool1(model.get_layer('average_pooling2d_1').output)
y=rev_actv1(y)
y=deconv1(y)
y=rev_actv2(y)
y=deconv2(y)
rev_model = keras.Model(inputs=[input],outputs=[y])

# Visualize
idx = 33290
sample = x_train[idx]
sample_predict = rev_model.predict(np.expand_dims(sample,axis=0),batch_size=1)[0]
sample_predict_norm = ((sample_predict - np.min(sample_predict))/(np.max(sample_predict)-np.min(sample_predict))*255).astype("int")
sample_predict_rgb = np.concatenate((sample_predict_norm,sample_predict_norm,sample_predict_norm),axis=2)
plt.imshow(sample_predict_rgb)