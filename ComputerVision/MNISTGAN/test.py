import torch.nn as nn
import torchvision
import torchvision.transforms as transforms
import torch
import torch.utils.data
import tqdm
import math
import numpy as np
import matplotlib.pyplot as plt
import matplotlib as mpl

mpl.rcParams['font.family'] = 'SimHei'

model = torch.load("./generator.pt")
fake_input = torch.randn((1, 28, 28)).to("cuda")
output = model(fake_input).detach().cpu().numpy()
output = output[0]
output = np.reshape(output,(28,28))
plt.subplot(1,2,1)
plt.title("网络输入 (Noise)")
plt.imshow(fake_input.detach().cpu().numpy()[0],cmap="gray")

plt.subplot(1,2,2)
plt.title("网络输出 (GAN/生成器)")
plt.imshow(output,cmap="gray")
plt.show()