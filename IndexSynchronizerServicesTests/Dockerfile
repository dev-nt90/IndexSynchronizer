# Initialize SQL Server and the AdventureWorks database
FROM chriseaton/adventureworks:latest
ARG BAK_FILE
ARG SA_PASSWORD
ENV SA_PASSWORD=${SA_PASSWORD}
EXPOSE 1433
WORKDIR /
COPY $BAK_FILE ./adventureworks.bak
COPY ./IndexSynchronizerServicesTests/install.sql .
COPY ./IndexSynchronizerServicesTests/CreateTestRunnerLogin.sql .
COPY ./IndexSynchronizerServicesTests/install.sh .
COPY ./IndexSynchronizerServicesTests/startup.sh .
RUN chmod +x ./install.sh
RUN chmod +x ./startup.sh
CMD /bin/bash ./startup.sh