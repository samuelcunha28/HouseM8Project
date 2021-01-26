import 'package:flutter/material.dart';
import 'package:housem8_flutter/helpers/storage_helper.dart';
import 'package:housem8_flutter/pages/favorite_mates_page.dart';
import 'package:housem8_flutter/pages/login_page.dart';
import 'package:housem8_flutter/pages/pending_jobs_employer_page.dart';
import 'package:housem8_flutter/pages/posts_list_page.dart';
import 'package:housem8_flutter/pages/update_employer_page.dart';
import 'package:housem8_flutter/pages/update_password_page.dart';
import 'package:housem8_flutter/view_models/employer_post_list_view.dart';
import 'package:housem8_flutter/view_models/employer_profile_view_model.dart';
import 'package:housem8_flutter/view_models/favorite_mates_list_view_model.dart';
import 'package:housem8_flutter/view_models/pending_job_list_view_model.dart';
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
              color: Color(0xFF93C901),
            ),
          ),
          ListTile(
            leading: Icon(Icons.border_color),
            title: Text('Editar Perfil'),
            onTap: () => {
              Navigator.of(context).push(
                MaterialPageRoute(
                    builder: (context) => ChangeNotifierProvider(
                          create: (context) => EmployerProfileViewModel(),
                          child: UpdateEmployerPage(),
                        )),
              )
            },
          ),
          ListTile(
              leading: Icon(Icons.lock),
              title: Text('Editar Palavra-Passe'),
              onTap: () => Navigator.of(context).push(
                    MaterialPageRoute(
                        builder: (context) =>
                            UpdatePasswordPage(role: "EMPLOYER")),
                  )),
          ListTile(
            leading: Icon(Icons.favorite),
            title: Text('Mates Favoritos'),
            onTap: () => {
              Navigator.of(context).push(
                MaterialPageRoute(
                    builder: (context) => ChangeNotifierProvider(
                          create: (context) => FavoriteMatesListViewModel(),
                          child: FavoriteMatesScreen(),
                        )),
              )
            },
          ),
          ListTile(
            leading: Icon(Icons.comment),
            title: Text('Publicações'),
            onTap: () => {
              Navigator.of(context).push(
                MaterialPageRoute(
                    builder: (context) => ChangeNotifierProvider(
                          create: (context) => EmployerPostListViewModel(),
                          child: EmployerPostsListPage(),
                        )),
              )
            },
          ),
          ListTile(
            leading: Icon(Icons.alarm),
            title: Text('Trabalhos Marcados'),
            onTap: () => {
              Navigator.of(context).push(MaterialPageRoute(
                  builder: (context) => ChangeNotifierProvider(
                        create: (context) => PendingJobListViewModel(),
                        child: PendingJobsEmployerPage(),
                      ))),
            },
          ),
          ListTile(
            leading: Icon(Icons.logout),
            title: Text('Logout'),
            onTap: () => {
              StorageHelper.deleteAllTokenData(),
              Navigator.pushReplacement(context,
                  MaterialPageRoute(builder: (context) => LoginScreen()))
            },
          )
        ],
      ),
    );
  }
}
