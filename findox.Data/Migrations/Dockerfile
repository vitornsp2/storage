FROM flyway/flyway:9.0.4-alpine

COPY ./*.sql /flyway/sql/
COPY ./flyway.conf /flyway/conf/

ENTRYPOINT [ "flyway", "migrate" ]