import tensorflow as tf
import numpy as np

# Shit!
# tf.compat.v1.disable_v2_behavior()

# Get Dataset
mnist=tf.keras.datasets.mnist
(x_train,y_train),(x_test,y_test)=mnist.load_data()
x_test = np.expand_dims(tf.keras.utils.normalize(x_test,axis=1),axis=3)
x_train = np.expand_dims(tf.keras.utils.normalize(x_train,axis=1),axis=3)

# Define Model
conv1 = tf.keras.layers.Conv2D(8,(3,3))
actv1 = tf.keras.layers.ReLU()
pool1 = tf.keras.layers.AvgPool2D()
conv2 = tf.keras.layers.Conv2D(16,(5,5))
actv2 = tf.keras.layers.ReLU()
pool2 = tf.keras.layers.AvgPool2D()
flatten = tf.keras.layers.Flatten()
dense1 = tf.keras.layers.Dense(256)
dense2 = tf.keras.layers.Dense(64)
dense3 = tf.keras.layers.Dense(10)
softmax = tf.keras.layers.Softmax()

input = tf.keras.layers.Input((28,28,1))
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

# Train
model = tf.keras.Model(inputs=[input],outputs=[x])
model.compile(optimizer='adam',loss='sparse_categorical_crossentropy',metrics=['accuracy'])
model.fit(x_train,y_train,epochs=10)
model.save("model_gradcam")