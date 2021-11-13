#########################
# Stage - base
## Source setup and Solution Restore
## COPY's over all the related files and run a dotnet restore on the solution
#########################
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /source

COPY *.sln .
COPY Directory.Build.props .
COPY src/. ./src/
COPY test/. ./test/

RUN dotnet restore


#########################
# Stage - base-build
## Build the Solution
## Runs dotnet build for solution, creating a base image for all projects.
#########################
FROM base AS base-build
ARG BUILD_CONFIGURATION=Release

RUN dotnet build --no-restore --configuration $BUILD_CONFIGURATION


#########################
# Stage - publish-main
## Publish the Main Server
## Runs dotnet publish for the Main Server.
#########################
FROM base-build AS publish-main
ARG BUILD_VERSION=0.0.0
ARG BUILD_CONFIGURATION=Release

RUN dotnet publish --configuration $BUILD_CONFIGURATION /p:Version=$BUILD_VERSION \
    --no-restore --no-build --output /app ./src/EventHorizon.Game.Server.Zone


#########################
# Stage - test-report-generation
## Test and Report Generation 
## Runs the test and reporting script, making it easier to run tests from inside a Container.
#########################
FROM base-build AS test-report-generation
ARG BUILD_VERSION=0.0.0
ARG BUILD_CONFIGURATION=Release
ENV BUILD_VERSION=${BUILD_VERSION}
ENV BUILD_CONFIGURATION=${BUILD_CONFIGURATION}

COPY _build-scripts/run-test-and-reporting.sh .

RUN chmod +x run-test-and-reporting.sh

ENTRYPOINT ["/source/run-test-and-reporting.sh"]


#########################
# Stage - server-scripts-sub-process
## Build the Server Script Sub Process 
## This allows for easier memory cleanup, since this will run in a sub process from the main server process.
#########################
FROM base-build AS server-scripts-sub-process
ARG BUILD_VERSION=0.0.0
ARG BUILD_CONFIGURATION=Release

## Single folder publish of project
RUN dotnet publish --output /sub-processes/server-scripts/ \
    --configuration $BUILD_CONFIGURATION /p:Version=$BUILD_VERSION \
    ./src/EventHorizon.Game.Server.Zone.Server.Scripts.SubProcess


#########################
# Stage - client-scripts-sub-process 
## Build the Client Script Sub Process 
## This allows for easier memory cleanup, since this will run in a sub process from the main server process.
#########################
FROM base-build AS client-scripts-sub-process
ARG BUILD_VERSION=0.0.0
ARG BUILD_CONFIGURATION=Release

## Single folder publish of whole project
RUN dotnet publish --no-restore --output /sub-processes/client-scripts/ \
    --configuration $BUILD_CONFIGURATION /p:Version=$BUILD_VERSION \
    ./src/EventHorizon.Game.Server.Zone.Client.Scripts.SubProcess


#########################
# Stage - runtime 
## Build the Runtime
## This is the main deployment image that can be used to deploy a fully production ready application.
#########################
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
ARG BUILD_VERSION=0.0.0
ENV APPLICATION_VERSION=$BUILD_VERSION

WORKDIR /app
COPY --from=publish-main /app /app
COPY --from=server-scripts-sub-process /sub-processes/server-scripts /sub-processes/server-scripts
COPY --from=client-scripts-sub-process /sub-processes/client-scripts /sub-processes/client-scripts
ENTRYPOINT ["dotnet", "EventHorizon.Game.Server.Zone.dll"]
