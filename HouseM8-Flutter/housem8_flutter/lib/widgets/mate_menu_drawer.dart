import 'package:flutter/material.dart';
import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:housem8_flutter/pages/calendar_page.dart';
import 'package:housem8_flutter/pages/login_page.dart';
import 'package:housem8_flutter/pages/mate_reviews_page.dart';
import 'package:housem8_flutter/pages/pending_jobs_mate_page.dart';
import 'package:housem8_flutter/pages/update_mate_page.dart';
import 'package:housem8_flutter/pages/update_password_page.dart';
import 'package:housem8_flutter/pages/work_categories_page.dart';
import 'package:housem8_flutter/view_models/categories_list_view_model.dart';
import 'package:housem8_flutter/view_models/mate_profile_view_model.dart';
import 'package:housem8_flutter/view_models/pending_job_list_view_model.dart';
import 'package:housem8_flutter/view_models/reviews_list_view_model.dart';
import 'package:provider/provider.dart';

class NavDrawer extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Drawer(
      child: ListView(
        padding: EdgeInsets.zero,
        children: <Widget>[
          DrawerHeader(
            child: Text(
              'Menu',
              style: TextStyle(color: Colors.white, fontSize: 30),
            ),
            decoration: BoxDecoration(
              color: Color(0xFF39A3ED),
            ),
          ),
          ListTile(
            leading: Icon(Icons.border_color),
            title: Text('Editar Perfil'),
            onTap: () => {
              Navigator.of(context).push(
                MaterialPageRoute(
                    builder: (context) => ChangeNotifierProvider(
                          create: (context) => MateProfileViewModel(),
                          child: UpdateMatePage(),
                        )),
              )
            },
          ),
          ListTile(
              leading: Icon(Icons.lock),
              title: Text('Editar Palavra-Passe'),
              onTap: () => Navigator.of(context).push(
                    MaterialPageRoute(
                        builder: (context) => UpdatePasswordPage(role: "MATE")),
                  )),
          ListTile(
            leading: Icon(Icons.work),
            title: Text('Categorias de Trabalho'),
            onTap: () => {
              Navigator.of(context).push(
                MaterialPageRoute(
                    builder: (context) => ChangeNotifierProvider(
                          create: (context) => CategoriesListViewModel(),
                          child: WorkCategoriesScreen(),
                        )),
              ),
            },
          ),
          ListTile(
            leading: Icon(Icons.alarm),
            title: Text('Trabalhos Pendentes'),
            onTap: () => {
              Navigator.of(context).push(
                MaterialPageRoute(
                    builder: (context) => ChangeNotifierProvider(
                          create: (context) => PendingJobListViewModel(),
                          child: PendingJobsMateScreen(),
                        )),
              ),
            },
          ),
          ListTile(
            leading: Icon(Icons.emoji_events),
            title: Text('Conquistas'),
            onTap: () => {Navigator.of(context).pop()},
          ),
          ListTile(
            leading: Icon(Icons.calendar_today),
            title: Text('Agenda Pessoal'),
            onTap: () => {
              Navigator.of(context).push(
                MaterialPageRoute(builder: (context) => CalendarScreen()),
              )
            },
          ),
          ListTile(
              leading: Icon(Icons.chat),
              title: Text('Consulta de Reviews'),
              onTap: () => Navigator.of(context).push(
                    MaterialPageRoute(
                        builder: (context) => ChangeNotifierProvider(
                              create: (context) => ReviewsListViewModel(),
                              child: MateReviewsPage(),
                            )),
                  )),
          ListTile(
              leading: Icon(Icons.logout),
              title: Text('Logout'),
              onTap: () => {
                    StorageHelper.deleteAllTokenData(),
                    Navigator.pushReplacement(context,
                        MaterialPageRoute(builder: (context) => LoginScreen()))
                  })
        ],
      ),
    );
  }
}
