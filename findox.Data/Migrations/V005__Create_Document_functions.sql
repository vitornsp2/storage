CREATE OR REPLACE FUNCTION storage.documents_create (
    filename TEXT,
    content_type TEXT,
    user_id BIGINT,
    description TEXT DEFAULT NULL,
    category TEXT DEFAULT NULL
)
    RETURNS SETOF storage.documents
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    RETURN QUERY
    INSERT
    INTO storage.documents (filename, content_type, description, category, user_id)
    VALUES ($1, $2, $4, $5, $3)
    RETURNING *;
END;
$$;

CREATE OR REPLACE FUNCTION storage.documents_content_create (
    document_id BIGINT,
    data BYTEA
)
    RETURNS SETOF storage.document_content
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    RETURN QUERY
    INSERT
    INTO storage.document_content (document_id, data)
    VALUES ($1, $2)
    RETURNING *;
END;
$$;

CREATE OR REPLACE FUNCTION storage.documents_get_all ()
    RETURNS SETOF storage.documents
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    RETURN QUERY
    SELECT *
    FROM storage.documents;
END;
$$;

CREATE OR REPLACE FUNCTION storage.documents_get_all_permitted (
    id_user BIGINT
)
    RETURNS SETOF storage.documents
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    RETURN QUERY
    SELECT storage.documents.*
    FROM storage.documents
    INNER JOIN storage.permits ON storage.documents.id = storage.permits.document_id
    WHERE storage.permits.user_id = $1
    OR storage.permits.group_id IN (
        SELECT group_id
        FROM storage.user_goru
        WHERE user_id = $1
    );
END;
$$;

CREATE OR REPLACE FUNCTION storage.documents_get_by_id (
    id BIGINT
)
    RETURNS SETOF storage.documents
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    RETURN QUERY
    SELECT *
    FROM storage.documents
    WHERE storage.documents.id = $1;
END;
$$;

CREATE OR REPLACE FUNCTION storage.documents_get_by_id_permitted (
    id_document BIGINT,
    id_user BIGINT
)
    RETURNS SETOF storage.documents
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    RETURN QUERY
    SELECT storage.documents.*
    FROM storage.documents
    INNER JOIN storage.permits ON storage.documents.id = storage.permits.document_id
    WHERE storage.documents.id = $1
    AND (
        storage.permits.user_id = $2
        OR storage.permits.group_id IN (
            SELECT group_id
            FROM storage.users_groups
            WHERE user_id = $2
        )
    );
END;
$$;

CREATE OR REPLACE FUNCTION storage.documents_content_get_by_document_id (
    id BIGINT
)
    RETURNS SETOF storage.document_content
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    RETURN QUERY
    SELECT *
    FROM storage.document_content
    WHERE storage.document_content.document_id = $1;
END;
$$;

CREATE OR REPLACE FUNCTION storage.documents_update (
    id BIGINT,
    filename TEXT DEFAULT NULL,
    description TEXT DEFAULT NULL,
    category TEXT DEFAULT NULL
)
    RETURNS BOOLEAN
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    UPDATE storage.documents
    SET filename = COALESCE($2, storage.documents.filename),
        description = COALESCE($3, storage.documents.description),
        category = COALESCE($4, storage.documents.category)
    WHERE storage.documents.id = $1;
    RETURN FOUND;
END;
$$;

CREATE OR REPLACE FUNCTION storage.documents_delete_by_id (
    id_value BIGINT
)
    RETURNS BOOLEAN
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    DELETE
    FROM storage.documents
    WHERE storage.documents.id = $1;
    RETURN FOUND;
END;
$$;

CREATE OR REPLACE FUNCTION storage.documents_content_delete_by_document_id (
    id BIGINT
)
    RETURNS BOOLEAN
    LANGUAGE PLPGSQL
    AS
$$
BEGIN
    DELETE
    FROM storage.document_content
    WHERE storage.document_content.document_id = $1;
    RETURN FOUND;
END;
$$;

CREATE OR REPLACE FUNCTION storage.documents_count_by_column_value_text (
    column_name TEXT,
    column_value TEXT
)
    RETURNS integer
    LANGUAGE PLPGSQL
    AS
$$
DECLARE
    row_count integer;
    query text := 'SELECT COUNT(*) FROM storage.documents';
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