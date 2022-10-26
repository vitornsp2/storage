CREATE OR REPLACE FUNCTION storage.permissions_create (
    document_id BIGINT,
    user_id BIGINT,
    group_id BIGINT
)
    RETURNS SETOF storage.permissions
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    RETURN QUERY
    INSERT
    INTO storage.permissions (document_id, user_id, group_id)
    VALUES ($1, $2, $3)
    RETURNING *;
END;
$$;

CREATE OR REPLACE FUNCTION storage.permissions_get_all ()
    RETURNS SETOF storage.permissions
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    RETURN QUERY
    SELECT *
    FROM storage.permissions;
END;
$$;

CREATE OR REPLACE FUNCTION storage.permissions_get_by_id (
    id_value BIGINT
)
    RETURNS SETOF storage.permissions
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    RETURN QUERY
    SELECT *
    FROM storage.permissions
    WHERE storage.permissions.id = $1;
END;
$$;

CREATE OR REPLACE FUNCTION storage.permissions_get_by_document_id (
    id_value BIGINT
)
    RETURNS SETOF storage.permissions
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    RETURN QUERY
    SELECT *
    FROM storage.permissions
    WHERE storage.permissions.document_id = $1;
END;
$$;

CREATE OR REPLACE FUNCTION storage.permissions_get_by_user_id (
    id_value BIGINT
)
    RETURNS SETOF storage.permissions
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    RETURN QUERY
    SELECT *
    FROM storage.permissions
    WHERE storage.permissions.user_id = $1
    OR storage.permissions.group_id IN (
        SELECT group_id
        FROM storage.users_groups
        WHERE user_id = $1
    );
END;
$$;

CREATE OR REPLACE FUNCTION storage.permissions_get_by_group_id (
    id_value BIGINT
)
    RETURNS SETOF storage.permissions
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    RETURN QUERY
    SELECT *
    FROM storage.permissions
    WHERE storage.permissions.group_id = $1;
END;
$$;

CREATE OR REPLACE FUNCTION storage.permissions_delete_by_id (
    id_value BIGINT
)
    RETURNS BOOLEAN
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    DELETE
    FROM storage.permissions
    WHERE storage.permissions.id = $1;
    RETURN FOUND;
END;
$$;

CREATE OR REPLACE FUNCTION storage.permissions_delete_by_document_id (
    id_value BIGINT
)
    RETURNS BOOLEAN
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    DELETE
    FROM storage.permissions
    WHERE storage.permissions.document_id = $1;
    RETURN FOUND;
END;
$$;

CREATE OR REPLACE FUNCTION storage.permissions_delete_by_user_id (
    id_value BIGINT
)
    RETURNS BOOLEAN
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    DELETE
    FROM storage.permissions
    WHERE storage.permissions.user_id = $1;
    RETURN FOUND;
END;
$$;

CREATE OR REPLACE FUNCTION storage.permissions_delete_by_group_id (
    id_value BIGINT
)
    RETURNS BOOLEAN
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    DELETE
    FROM storage.permissions
    WHERE storage.permissions.group_id = $1;
    RETURN FOUND;
END;
$$;

CREATE OR REPLACE FUNCTION storage.permission_match_count (
    id_document BIGINT,
    id_user BIGINT,
    id_group BIGINT
)
    RETURNS integer
    LANGUAGE PLPGSQL
    AS
$$
DECLARE
    row_count integer;
    query text := 'SELECT COUNT(*) FROM storage.permissions WHERE storage.permissions.document_id = $1';
BEGIN
    IF id_user IS NOT NULL THEN
        query := query || ' AND storage.permissions.user_id = $2';
    END IF;
    IF id_group IS NOT NULL THEN
        query := query || ' AND storage.permissions.group_id = $3';
    END IF;
    EXECUTE query
    USING id_document, id_user, id_group
    INTO row_count;
    RETURN row_count;
END;
$$;

CREATE OR REPLACE FUNCTION storage.permissions_count_by_column_value_id (
    column_name TEXT,
    column_value BIGINT
)
    RETURNS integer
    LANGUAGE PLPGSQL
    AS
$$
DECLARE
    row_count integer;
    query text := 'SELECT COUNT(*) FROM storage.permissions';
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