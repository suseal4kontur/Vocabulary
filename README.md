# Vocabulary API
This API is designed for keeping track of English words and phrases you learn. Entries into the vocabulary are made by lemmas.
## Requests
### Entries:
* GET /entries/{lemma}
* GET /entries?partOfSpeech={partOfSpeech}&form={form}&synonym={synonym}&fromAddedAt={fromAddedAt}&toAddedAt={toAddedAt}&offset={offset}&limit={limit}
* POST /entries/
* PATCH /entries/{lemma}
* DELETE /entries/{lemma}
### EntriesByMeaning
* GET /entriesByMeaning/{meaningId}
### Meanings
* POST /entries/{lemma}/meanings/
* PATCH /entries/{lemma}/meanings/{meaningId}
* DELETE /entries/{lemma}/meanings/{meaningId}
## Status codes
| Code            | Description                                                                             |
| --------------- | --------------------------------------------------------------------------------------- |
| 200 OK          | The GET or POST request was successful, and an entry or meaning is returned as JSON.    |
| 204 No Content  | The PATCH or DELETE request was successful, and there is no additional content to send. |
| 400 Bad Request | The request's data is incorrect, errors are provided as JSON.                           |
| 404 Not Found   | The entry/meaning cannot be found by the provided lemma/meaning ID.                     |
