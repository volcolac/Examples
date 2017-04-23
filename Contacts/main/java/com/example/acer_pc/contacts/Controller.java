package com.example.acer_pc.contacts;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import java.util.ArrayList;
import java.util.List;

public class Controller {
    public List<Contact> contacts;
    DBHelper dbHelper;
    SQLiteDatabase database;

    public Controller(DBHelper dbh) {
        dbHelper = dbh;
        database = dbHelper.getWritableDatabase();
        Cursor cursor = database.query(DBHelper.TABLE_CONTACTS, null, null, null, null, null, null);
        contacts = new ArrayList<Contact>();
        if (cursor.moveToFirst()) {
            int nameIndex = cursor.getColumnIndex(DBHelper.KEY_NAME);
            int numberIndex = cursor.getColumnIndex(DBHelper.KEY_NUMBER);
            do {
                contacts.add(new Contact(cursor.getString(nameIndex), cursor.getString(numberIndex)));
            } while (cursor.moveToNext());
        }
        cursor.close();
        dbHelper.close();
        _sort("");
    }

    public void _sort(String s) {
        if (s == "") {
            for (int i = 0; i < contacts.size() - 1; i++)
                for (int j = i + 1; j < contacts.size(); j++) {
                    if (contacts.get(i).name.compareTo(contacts.get(j).name) > 0) {
                        Contact h = contacts.get(i);
                        contacts.set(i, contacts.get(j));
                        contacts.set(j, h);
                    }
                }
        } else {
            for (int i = 0; i < contacts.size() - 1; i++)
                for (int j = i + 1; j < contacts.size(); j++) {
                    int i1 = contacts.get(i).name.indexOf(s);
                    int i2 = contacts.get(j).name.indexOf(s);
                    if (i1 == i2 && contacts.get(i).name.compareTo(contacts.get(j).name) > 0) {
                        Contact h = contacts.get(i);
                        contacts.set(i, contacts.get(j));
                        contacts.set(j, h);
                        continue;
                    }
                    if (i1 == -1 && i2 != -1) {
                        Contact h = contacts.get(i);
                        contacts.set(i, contacts.get(j));
                        contacts.set(j, h);
                        continue;
                    }
                    if (i1 != -1 && i2 != -1 && i1 > i2) {
                        Contact h = contacts.get(i);
                        contacts.set(i, contacts.get(j));
                        contacts.set(j, h);
                        continue;
                    }
                }
        }
    }
}
