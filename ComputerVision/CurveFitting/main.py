from turtle import forward
import torch
import torch.nn as nn
import numpy as np

from torch.autograd import Variable
from torch.nn import Parameter
import math

import matplotlib.pyplot as plt

def comb(n,m):
    if n==m:
        return 1
    elif m==0:
        return 1
    elif n>m and n>=0 and m>=0:
        return comb(n-1,m-1)+comb(n-1,m)
    else:
        print(n,m)
        raise

def visualize_gt(p):
    x = [p[i][0] for i in range(len(p))]
    y = [p[i][1] for i in range(len(p))]
    plt.plot(x,y,'b',label="Reference")

def visualize_pr(p):
    x = [p[i][0].detach().cpu().numpy() for i in range(len(p))]
    y = [p[i][1].detach().cpu().numpy() for i in range(len(p))]
    plt.plot(x,y,'orange',label="Approximated")

class StraightLineModel(nn.Module):
    def __init__(self):
        super().__init__()
        self.theta = Parameter(torch.FloatTensor([3.14/2]))
        self.start_x = Parameter(torch.FloatTensor([0]))
        self.start_y = Parameter(torch.FloatTensor([0]))

    def forward(self, t):
        pos_x = self.start_x + torch.cos(self.theta) * t
        pos_y = self.start_y + torch.sin(self.theta) * t
        return (pos_x,pos_y)

class BezierLineModel(nn.Module):
    def __init__(self,order):
        super().__init__()
        self.order = order
        self.comb_coef = [comb(order,i) for i in range(order+1)]
        self.px =  nn.ParameterList([Parameter(torch.FloatTensor([1])) for i in range(order+1)])
        self.py =  nn.ParameterList([Parameter(torch.FloatTensor([1])) for i in range(order+1)])
        self.sx = 0
        self.sy = 1
        self.pt = 0

    def set_starting(self,x,y,p):
        self.sx = x
        self.sy = y
        self.pt = p
    
    def forward(self,t):
        t = (t-self.pt)/(1-self.pt)
        posx = 0
        posy = 0
        for i in range(self.order+1):
            if i==0:
                posx += self.comb_coef[i] * self.sx * ((1-t)**(self.order-i))*(t**i)
                posy += self.comb_coef[i] * self.sy * ((1-t)**(self.order-i))*(t**i)
            else:
                posx += self.comb_coef[i] * self.px[i] * ((1-t)**(self.order-i))*(t**i)
                posy += self.comb_coef[i] * self.py[i] * ((1-t)**(self.order-i))*(t**i)
        return (posx,posy)

class StraightCurvedLineModel(nn.Module):
    def __init__(self, straight, bent):
        super().__init__()
        self.straight = straight
        self.bent = bent
        self.partition = Parameter(torch.FloatTensor([0.5]))

    def forward(self, t):
        straight_mask = t < self.partition
        straight_weight = self.straight(t)
        end_idx = 0
        for i in range(len(t)):
            if t[i]< self.partition:
                end_idx = i
        self.bent.set_starting(straight_weight[0][end_idx],straight_weight[1][end_idx],self.partition)
        bent_mask = t>=self.partition
        bent_weight = self.bent(t)
        ret = [(straight_weight[0][i]*straight_mask[i]+bent_weight[0][i]*bent_mask[i],
                straight_weight[1][i]*straight_mask[i]+bent_weight[1][i]*bent_mask[i]) for i in range(len(t))]
        return ret

class DiscreteSSELoss(nn.Module):
    def __init__(self):
        super().__init__()

    def forward(self,pr,gt):
        dist = 0
        for i in range(len(pr)):
            deviation_x = (pr[i][0] - gt[i][0]) ** 2
            deviation_y = (pr[i][1] - gt[i][1]) ** 2
            dist += deviation_x + deviation_y
        return dist

class CustomLineA(nn.Module):
    def __init__(self):
        super().__init__()
    
    def forward(self,t):
        ret = []
        for i in range(len(t)):
            if t[i]<=0.5:
                ret.append((0,t[i]))
            else:
                dt = (3.14159 - (t[i]-0.5)/0.5 * 3.14159/2)
                dx = (math.cos(dt) + 1)/2
                dy = (math.sin(dt))/2 + 0.5
                ret.append((dx,dy))
        return ret



if __name__ == "__main__":

    t = torch.linspace(0,1)
    st = StraightLineModel()
    cr = BezierLineModel(2)
    print(cr.parameters)
    sf = StraightCurvedLineModel(st,cr)
    gt = CustomLineA()
    optim = torch.optim.SGD(sf.parameters(),lr=5e-3)
    gtl = gt(t)
    loss = DiscreteSSELoss()
    for i in range(1000):
        optim.zero_grad()
        pr = sf(t)
        ls = loss(pr,gtl)
        ls.backward()
        optim.step()
        print("Iteration ",i,"Loss=",ls.detach().cpu().numpy())
    pr = sf(t)
    #print(pr)
    plt.figure()
    visualize_gt(gtl)
    visualize_pr(pr)
    print("PARTITION=",sf.partition)

    plt.legend()
    plt.show()