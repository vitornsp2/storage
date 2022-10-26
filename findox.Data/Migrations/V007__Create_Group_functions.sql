CREATE OR REPLACE FUNCTION storage.groups_create (
    name TEXT,
    description TEXT DEFAULT NULL
)
    RETURNS SETOF storage.groups
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    RETURN QUERY
    INSERT
    INTO storage.groups (name, description)
    VALUES ($1, $2)
    RETURNING *;
END;
$$;

CREATE OR REPLACE FUNCTION storage.users_groups_create (
    group_id BIGINT,
    user_id BIGINT
)
    RETURNS SETOF storage.users_groups
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    RETURN QUERY
    INSERT
    INTO storage.users_groups (group_id, user_id)
    VALUES ($1, $2)
    RETURNING *;
END;
$$;

CREATE OR REPLACE FUNCTION storage.groups_get_all ()
    RETURNS SETOF storage.groups
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    RETURN QUERY
    SELECT *
    FROM storage.groups;
END;
$$;

CREATE OR REPLACE FUNCTION storage.users_groups_get_all ()
    RETURNS SETOF storage.users_groups
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    RETURN QUERY
    SELECT *
    FROM storage.users_groups;
END;
$$;

CREATE OR REPLACE FUNCTION storage.groups_get_by_id (
    id_value BIGINT
)
    RETURNS SETOF storage.groups
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    RETURN QUERY
    SELECT *
    FROM storage.groups
    WHERE storage.groups.id = $1;
END;
$$;

CREATE OR REPLACE FUNCTION storage.users_groups_get_by_id (
    id_value BIGINT
)
    RETURNS SETOF storage.users_groups
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    RETURN QUERY
    SELECT *
    FROM storage.users_groups
    WHERE storage.users_groups.id = $1;
END;
$$;

CREATE OR REPLACE FUNCTION storage.users_groups_get_by_group_id (
    id_value BIGINT
)
    RETURNS SETOF storage.users_groups
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    RETURN QUERY
    SELECT *
    FROM storage.users_groups
    WHERE storage.users_groups.group_id = $1;
END;
$$;

CREATE OR REPLACE FUNCTION storage.users_groups_get_by_user_id (
    id_value BIGINT
)
    RETURNS SETOF storage.users_groups
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    RETURN QUERY
    SELECT *
    FROM storage.users_groups
    WHERE storage.users_groups.user_id = $1;
END;
$$;

CREATE OR REPLACE FUNCTION storage.groups_update (
    id BIGINT,
    name TEXT DEFAULT NULL,
    description TEXT DEFAULT NULL
)
    RETURNS BOOLEAN
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    UPDATE storage.groups
    SET name = COALESCE($2, storage.groups.name),
        description = COALESCE($3, storage.groups.description)
    WHERE storage.groups.id = $1;
    RETURN FOUND;
END;
$$;

CREATE OR REPLACE FUNCTION storage.groups_delete_by_id (
    id_value BIGINT
)
    RETURNS BOOLEAN
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    DELETE
    FROM storage.groups
    WHERE storage.groups.id = $1;
    RETURN FOUND;
END;
$$;

CREATE OR REPLACE FUNCTION storage.users_groups_delete_by_id (
    id_value BIGINT
)
    RETURNS BOOLEAN
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    DELETE
    FROM storage.users_groups
    WHERE storage.users_groups.id = $1;
    RETURN FOUND;
END;
$$;

CREATE OR REPLACE FUNCTION storage.users_groups_delete_by_group_id (
    id_value BIGINT
)
    RETURNS BOOLEAN
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    DELETE
    FROM storage.users_groups
    WHERE storage.users_groups.group_id = $1;
    RETURN FOUND;
END;
$$;

CREATE OR REPLACE FUNCTION storage.users_groups_delete_by_user_id (
    id_value BIGINT
)
    RETURNS BOOLEAN
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    DELETE
    FROM storage.users_groups
    WHERE storage.users_groups.user_id = $1;
    RETURN FOUND;
END;
$$;