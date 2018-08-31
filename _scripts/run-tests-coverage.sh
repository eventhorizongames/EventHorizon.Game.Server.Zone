docker build --target testrunner -t ehz/game/server/zone:test .;
docker run --rm -v "$(pwd)"/TestResults:/source/test/TestResults ehz/game/server/zone:test;