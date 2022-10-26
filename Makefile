#set enviroment variables
	export STORAGE_CRYPTO_KEY := MqSm0P5dMgFSZhEBKpCv4dVKgDrsgrmT 
.PHONY: build
build:	   
	docker-compose build
.PHONY: run
run:	   
	docker-compose up
.PHONY: test
test:	                
	@# This will execute unit tests, after making sure the development database is up to date with migrations.
	# ----------
	# Running unit tests.
	dotnet test --verbosity quiet /p:CollectCoverage=true /p:CoverletOutputFormat=opencover