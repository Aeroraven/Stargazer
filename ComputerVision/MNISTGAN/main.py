import torch.nn as nn
import torchvision
import torchvision.transforms as transforms
import torch
import torch.utils.data
import tqdm
import math

# Load Data
bsz = 32
device="cuda"
transform = transforms.Compose([
    transforms.ToTensor(),
    transforms.Normalize(mean=(0.5),
                         std=(0.5))
])
mnist_data = torchvision.datasets.MNIST("./mnist_data",train=True, download=True, transform=transform)
mnist_dataloader = torch.utils.data.DataLoader(mnist_data,batch_size=bsz)
print(mnist_data.data.shape)

# Discriminator
discriminator = nn.Sequential(
    nn.Linear(28 * 28, 28 * 28),
    nn.ReLU(),
    nn.Linear(28 * 28, 28 * 28),
    nn.ReLU(),
    nn.Linear(28 * 28, 1),
    nn.Sigmoid(),
).to(device)

# Generator
generator = nn.Sequential(
    nn.Flatten(),
    nn.Linear(28 * 28, 28 * 28),
    nn.ReLU(),
    nn.Linear(28 * 28, 28 * 28),
    nn.ReLU(),
    nn.Linear(28 * 28, 28 * 28),
    nn.Tanh(),
).to(device)

# Params
generator_optim = torch.optim.Adam(generator.parameters(), lr=1e-4)
discriminator_optim = torch.optim.Adam(discriminator.parameters(), lr=1e-4)
loss_fn = torch.nn.BCELoss()

# Train
for epoch in range(100):
    print("Epoch", epoch)
    with tqdm.tqdm(total=math.ceil(len(mnist_data)/bsz),desc="Training Phase") as t:
        for i, (image, label) in enumerate(mnist_dataloader):
            image = image.to(device)
            label = label.to(device)

            image = image.reshape((bsz,28*28))
            # Discriminate true samples
            true_label = torch.ones((bsz,1)).to(device)
            real_output = discriminator(image)
            real_loss = loss_fn(real_output, true_label)

            # Discriminate fake samples
            fake_label = torch.zeros((bsz,1)).to(device)
            fake_input = torch.randn((bsz, 28, 28)).to(device)
            fake_image = generator(fake_input)
            fake_output = discriminator(fake_image)
            fake_loss = loss_fn(fake_output, fake_label)

            # Back-propagate discriminator loss
            generator.zero_grad()
            discriminator.zero_grad()
            discriminator_loss = real_loss + fake_loss
            discriminator_loss.backward()
            discriminator_optim.step()

            # Train generator
            fake_label = torch.zeros((bsz,1)).to(device)
            fake_input = torch.randn((bsz, 28, 28)).to(device)
            fake_image = generator(fake_input)
            fake_output = discriminator(fake_image)
            fake_loss_g = loss_fn(fake_output, true_label)
            generator.zero_grad()
            discriminator.zero_grad()
            fake_loss_g.backward()
            generator_optim.step()

            t.set_postfix(GLoss=fake_loss_g.detach().cpu().numpy(),DLoss=discriminator_loss.detach().cpu().numpy())
            t.update(1)

    torch.save(discriminator,"./discriminator.pt")
    torch.save(generator,"./generator.pt")
