import cv2
import matplotlib.pyplot as plt
import matplotlib as mpl
import numpy as np
import math

if __name__ == "__main__":
    mpl.rcParams['font.family'] = 'SimHei'
    thresh = 160
    bloom_weight = 1.5
    kernel_size_f = 4

    a = cv2.imread("a.jpg")
    a = cv2.cvtColor(a,cv2.COLOR_BGR2RGB)
    sz = a.shape

    ksz = int(math.sqrt(min(sz[0],sz[1])))
    ai = np.maximum(a[:,:,0],a[:,:,1])
    ai = np.maximum(a[:,:,2],ai)

    ai[ai<thresh] = 0
    ai[ai>=thresh] = 1
    
    ais = a.copy()
    for i in range(3):
        ais[:,:,i] = ai * a[:,:,i]

    #aig = (cv2.GaussianBlur(ais,(kernel_size_f*ksz+1,kernel_size_f*ksz+1),85)/2 + ais/2)
    aig = cv2.GaussianBlur(ais,(kernel_size_f*ksz+1,kernel_size_f*ksz+1),85)
    aig = aig / 255 

    aigf = a.copy().astype("float64") / 255
    aigf = aigf * aig
    
    aigc = aig * bloom_weight + a.astype("float64") / 255
    aigc[aigc>1] = 1
    aigc[aigc<0] = 0


    plt.subplot(2,2,1)
    plt.title("输入")
    plt.imshow(a)
    plt.subplot(2,2,2)
    plt.title("输出 Bloom(高斯滤波)")
    plt.imshow(aigc)
    plt.subplot(2,2,3)
    plt.title("滤波响应")
    plt.imshow(aig * bloom_weight,cmap="gray")
    plt.subplot(2,2,4)
    plt.title("阈值")
    plt.imshow(ai,cmap="gray")
    plt.show()
