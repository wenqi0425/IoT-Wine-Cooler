from sense_hat import SenseHat

sense = SenseHat()

sense.clear()

red = (255, 0, 0)
green = (0,255,0) 

temp = sense.get_temperature() 
print(temp)

desiredTemp = 12

diff = temp - desiredTemp
print(diff)

if diff > 0:
  sense.show_message("", back_colour = red)
  if diff > 10:
    sense.show_message("WARNING", back_colour = red)
else:
  sense.show_message("", back_colour = green)