from pymouse import PyMouse
import time
playX = 50
playY = 1020

forwardX = 260
forwardY = 1020

centerX = 200
centerY = 200


m = PyMouse()



def wait_for_hide():
    m.move(x=centerX,y=centerY)
    time.sleep(4)

def play_pause():
    m.click(button=1,x=playX,y=playY)
    time.sleep(0.52)

def forward(n=1):
    for i in range(n):
        m.click(button=1,x=forwardX,y=forwardY)
        time.sleep(0.52)
