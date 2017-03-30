NUMBER_OF_DOORS = 100
doors = [False] * NUMBER_OF_DOORS


def toggle_doors():
	try:
		for i in range(0, NUMBER_OF_DOORS):
			for j in range(0, NUMBER_OF_DOORS+1):
				if not (i+1)%(j+1):
					doors[i] = not doors[i]
	except error: #this is in case the file is modified from the original state, in which there are no exceptions
		print "There was an exception when doing the pass throughs of the doors: ", error


def print_all_doors():
	print "The state of all doors is as follows:"
	try:
		for i in range(0, NUMBER_OF_DOORS):
			print "Door", i+1, "is", ("open" if doors[i] else "closed")
	except error: #this is in case the file is modified from the original state, in which there are no exceptions
		print "There was an exception when printing the status of the doors: ", error


#this function is not used at the moment but could be later so it will stay here for now
def print_doors(opened):
	try:
		if not type(opened) is bool:
			print "Please enter a boolean for this function's parameter"
			return
		print "The following doors are", ("open" if open else "closed")
		for i in range(0, NUMBER_OF_DOORS):
			if doors[i]==opened:
				print i+1,
		print #this is simply for better looking output if something is printed after this function
	except error: #this is in case the file is modified from the original state, in which there are no exceptions
		print "There was an exception when printing the doors that are ", ("open" if open else "closed") ,": ", error


def main():
	print "Begin 100 Doors program"
	print "Running pass throughs as explained in tasks.txt..."
	toggle_doors()
	#print_all_doors() #this function is much more verbose, and is commented in case the functionality is requested
	print_doors(True)
	print "End 100 Doors Program"


if __name__ == "__main__":
	main()