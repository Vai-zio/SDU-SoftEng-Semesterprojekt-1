TARGETS = \
	woz.zip \


all: ${TARGETS}

clean:
	touch dummy~
	rm        *~

mrproper: clean
	touch  bin obj
	rm -Rf bin obj


woz.zip: *.cs
	make mrproper
	mkdir woz
	cp *.cs woz
	zip -r woz.zip woz
	rm -Rf woz

