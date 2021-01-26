import 'package:flutter/material.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:housem8_flutter/helpers/connection_helper.dart';
import 'package:housem8_flutter/models/mate_review.dart';
import 'package:housem8_flutter/services/mate_review_service.dart';
import 'package:housem8_flutter/widgets/error_message_dialog.dart';
import 'package:smooth_star_rating/smooth_star_rating.dart';

class MateReviewPage extends StatefulWidget {
  final int mateId;

  const MateReviewPage({Key key, this.mateId}) : super(key: key);

  @override
  _MateReviewScreen createState() => _MateReviewScreen();
}

class _MateReviewScreen extends State<MateReviewPage> {
  double _rating = 0.0;
  String _comment;

  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();

  Widget ratingWidget() {
    return Container(
      padding: EdgeInsets.fromLTRB(16.0, 20.0, 0.0, 0.0),
      child: SmoothStarRating(
        rating: _rating,
        isReadOnly: false,
        size: 30,
        color: Colors.blue,
        borderColor: Colors.blue,
        filledIconData: Icons.star,
        halfFilledIconData: Icons.star_half,
        defaultIconData: Icons.star_border,
        starCount: 5,
        allowHalfRating: true,
        spacing: 2.0,
        onRated: (newRating) {
          _rating = newRating;
        },
      ),
    );
  }

  Widget commentWidget() {
    return TextFormField(
      decoration: InputDecoration(
          labelText: 'Comentário',
          helperText: 'Resume brevemente a sua experiência com o trabalhador'),
      maxLength: 50,
      validator: (String value) {
        if (value.length == 0) {
          return "Introduza uma descrição";
        } else if (value.length < 10) {
          return "A descrição deve ter no minimo 10 caracteres";
        }
        return null;
      },
      onSaved: (String value) {
        _comment = value;
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text("Review de mate")),
      body: SingleChildScrollView(
        child: Container(
          margin: EdgeInsets.all(24),
          child: Form(
            key: _formKey,
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: <Widget>[
                Container(
                  padding: EdgeInsets.fromLTRB(16.0, 20.0, 0.0, 0.0),
                  child: Text('Avalie a sua experiência com o trabalhador',
                      style: TextStyle(
                          fontSize: 20.0,
                          fontWeight: FontWeight.bold,
                          color: Colors.blue)),
                ),
                ratingWidget(),
                commentWidget(),
                SizedBox(height: 30),
                RaisedButton(
                  padding: EdgeInsets.all(15.0),
                  shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(30.0)),
                  color: Colors.white,
                  child: Text(
                    'Avaliar',
                    style: TextStyle(color: Colors.blue, fontSize: 16),
                  ),
                  onPressed: () async {
                    if (!_formKey.currentState.validate()) {
                      return;
                    }
                    _formKey.currentState.save();
                    await actionReviewMate();

                    //Send to API
                  },
                )
              ],
            ),
          ),
        ),
      ),
    );
  }

  // passar os dados
  Future<void> actionReviewMate() async {
    if (await ConnectionHelper.checkConnection()) {
      MateReviews newRating =
          new MateReviews(rating: _rating, comment: _comment);

      // mudar para id do mate
      var statusCode =
          await MateReviewService().createMateReview(widget.mateId, newRating);

      if (statusCode == 200) {
        Navigator.pop(context);
        Fluttertoast.showToast(
            msg: "Obrigado pela sua avaliação!",
            toastLength: Toast.LENGTH_LONG,
            gravity: ToastGravity.BOTTOM,
            timeInSecForIosWeb: 5,
            backgroundColor: Colors.lightBlue,
            textColor: Colors.white,
            fontSize: 16.0);
      }
    } else {
      showDialog(
        context: context,
        builder: (context) => ErrorMessageDialog(
            title: "Sem conexão",
            text: "Dispositivo não consegue conectar ao servidor!"),
      );
    }
  }
}
