package com.example.acer_pc.contacts;
import android.content.ContentValues;
import android.database.sqlite.SQLiteDatabase;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.View;
import android.widget.Button;
import android.widget.GridLayout;
import android.widget.TextView;

public class MainActivity extends AppCompatActivity {

    private GridLayout.LayoutParams params;
    private TextView textView;
    private Button delete;
    private Controller controller;
    private GridLayout content;
    private TextView editFind;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        controller = new Controller(new DBHelper(this));
        content = (GridLayout) findViewById(R.id.mainContent);
        editFind = (TextView) findViewById(R.id.editFind);
        editFind.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {}
            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {
                controller._sort(editFind.getText().toString());
                refresh();
            }
            @Override
            public void afterTextChanged(Editable s) {}
        });
        Button plus = (Button) findViewById(R.id.add);
        final TextView name = (TextView) findViewById(R.id.editName);
        final TextView number = (TextView) findViewById(R.id.editNumber);
        plus.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String sn = name.getText().toString();
                name.setText("");
                String snum = number.getText().toString();
                number.setText("");
                Add(sn, snum);
                refresh();
            }
        });
        refresh();
    }

    private void writeContact(final int id) {
        delete = new Button(this);
        delete.setText("-");
        params = new GridLayout.LayoutParams();
        params.width = 60;
        delete.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Delete(id);
                refresh();
            }
        });
        content.addView(delete, params);
        textView = new TextView(this);
        textView.setText(controller.contacts.get(id).name);
        textView.setTextSize(24);
        params = new GridLayout.LayoutParams();
        params.rightMargin = 16;
        params.topMargin = 16;
        content.addView(textView, params);
        textView = new TextView(this);
        textView.setText(controller.contacts.get(id).number);
        textView.setTextSize(24);
        params = new GridLayout.LayoutParams();
        params.rightMargin = 16;
        params.topMargin = 16;
        content.addView(textView, params);
    }

    private void refresh() {
        content.removeAllViews();
        for(int i = 0; i < controller.contacts.size(); i++) {
            writeContact(i);
        }
    }

    public void Delete(int id) {
        DBHelper dbHelper = new DBHelper(this);
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        Contact cur = controller.contacts.get(id);
        controller.contacts.remove(id);
        String s = DBHelper.KEY_NAME + "= ? AND " + DBHelper.KEY_NUMBER + "= ?";
        database.delete(DBHelper.TABLE_CONTACTS, s, new String[]{cur.name, cur.number});
        dbHelper.close();
    }

    public void Add(String name, String number) {
        DBHelper dbHelper = new DBHelper(this);
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        ContentValues contentValues = new ContentValues();
        contentValues.put(DBHelper.KEY_NAME, name);
        contentValues.put(DBHelper.KEY_NUMBER, number);
        database.insert(DBHelper.TABLE_CONTACTS, null, contentValues);
        dbHelper.close();
        controller.contacts.add(new Contact(name, number));
        controller._sort("");
    }
}
