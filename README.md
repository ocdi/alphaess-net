This project is inspired by https://github.com/CharlesGillanders/alphaess/

I don't know python and want to write my own controller to pre-charge my battery on hours of my choosing. There are limitations that if you set it to charge to X% between certain hours, and PV happens to push past it, it will stop charging and send the excess to the grid. Not the worst issue, but I would rather the battery keep the charge.

In Victoria, where I live, our power plans have peak hours of 15:00 to 21:00. This means that it is possible we have a sunny day and we get to 100% early, but if it is cloudy the battery might be quite low prior to peak period. We know when the sun will set, and the general curve of potential PV so we can estimate how much to pre-charge the battey prior to peak starting.

I'm planning on writing a program that monitors the charge during the day and depending on how things go, to instruct the battery to pre-charge starting from 12:00 (for really cloudy days, to 14:00 for partially sunny days). The idea is to tune it, such that we get to our target percentage by 15:00, allowing us to reduce the amount of peak power usage.

