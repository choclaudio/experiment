docker build -t serverserver:latest -f ockerfile.serverserver 
docker build -t nginx-vnc-cloud:latest -f Dockerfile.nginx-vnc-cloud
docker build -t novnc:latest -f Dockerfile.novnc