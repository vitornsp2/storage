CREATE OR REPLACE FUNCTION storage.users_create (
    name VARCHAR( 50 ),
    password TEXT,
    email TEXT,
    role TEXT
)
    RETURNS SETOF storage.users
    LANGUAGE PLPGSQL
    AS
$$
DECLARE
    saltedhash TEXT;
BEGIN
    SELECT crypt($2, gen_salt('bf', 8))
    INTO saltedhash;

    RETURN QUERY
    INSERT
    INTO storage.users (name, password, email, role)
    VALUES ($1, saltedhash, $3, $4)
    RETURNING *;
END;
$$;

CREATE OR REPLACE FUNCTION storage.users_get_all()
    RETURNS SETOF storage.users
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    RETURN QUERY
    SELECT *
    FROM storage.users;
END;
$$;

CREATE OR REPLACE FUNCTION storage.users_get_by_id (
    id BIGINT
)
    RETURNS SETOF storage.users
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    RETURN QUERY
    SELECT *
    FROM storage.users
    WHERE storage.users.id = $1;
END;
$$;

CREATE OR REPLACE FUNCTION storage.users_update (
    id BIGINT,
    name VARCHAR( 100 ) DEFAULT NULL,
    email TEXT DEFAULT NULL,
    role TEXT DEFAULT NULL
)
    RETURNS BOOLEAN
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    UPDATE storage.users
    SET name = COALESCE($2, storage.users.name),
        email = COALESCE($3, storage.users.email),
        role = COALESCE($4, storage.users.role)
    WHERE storage.users.id = $1;
    RETURN FOUND;
END;
$$;

CREATE OR REPLACE FUNCTION storage.users_delete_by_id (
    id BIGINT
)
    RETURNS BOOLEAN
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    DELETE
    FROM storage.users
    WHERE storage.users.id = $1;
    RETURN FOUND;
END;
$$;

CREATE OR REPLACE FUNCTION storage.users_authenticate (
    email TEXT,
    password TEXT
)
    RETURNS SETOF storage.users
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    RETURN QUERY
    SELECT *
    FROM storage.users
    WHERE storage.users.email = $1
        AND storage.users.password = crypt($2, storage.users.password);
END;
$$;

CREATE OR REPLACE FUNCTION storage.users_update_password (
    id BIGINT,
    password TEXT
)
    RETURNS BOOLEAN
    LANGUAGE PLPGSQL
    AS
$$
DECLARE
    saltedhash TEXT;
BEGIN
    SELECT crypt($2, gen_salt('bf', 8)) INTO saltedhash;

    UPDATE storage.users
    SET password = saltedhash
    WHERE storage.users.id = $1;
    RETURN FOUND;
END;
$$;

CREATE OR REPLACE FUNCTION storage.users_count_by_column_value_text (
    column_name TEXT,
    column_value TEXT
)
    RETURNS integer
    LANGUAGE PLPGSQL
    AS
$$
DECLARE
    row_count integer;
    query text := 'SELECT COUNT(*) FROM storage.users';
BEGIN
    IF column_name IS NOT NULL THEN
        query := query || ' WHERE ' || quote_ident(column_name) || ' = $1';
    END IF;
    EXECUTE query
    USING column_value
    INTO row_count;
    RETURN row_count;
END;
$$;