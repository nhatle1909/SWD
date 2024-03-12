import { linkImg } from '@/utils/common'
import { useAppDispatch, useAppSelector } from '../../store';
import { useEffect } from 'react';
import { actionGetInteriosList } from '../../store/interior/action';

const Home = () => {
	const dispatch = useAppDispatch();
	const interiors = useAppSelector(({ interiors }) => interiors.interiors);
	console.log('check interiors::', interiors)
	useEffect(() => {
		dispatch(actionGetInteriosList({
			pageIndex: 1,
			isAsc: true,
			searchValue: ''
		}))
	}, [])
	console.log("interiors", interiors);
	return (
		<>

			<section className="home-slider js-fullheight owl-carousel">
				<div className="slider-item js-fullheight" style={{ backgroundImage: `url(${linkImg('bg_1.jpg')})` }}>
					<div className="overlay"></div>
					<div className="container">
						<div className="row no-gutters slider-text js-fullheight align-items-center justify-content-end" data-scrollax-parent="true">
							<div className="col-md-7 text ftco-animate" data-scrollax=" properties: { translateY: '70%' }">
								<h1 className="mb-4" data-scrollax="properties: { translateY: '30%', opacity: 1.6 }">We Create Amazing Architecture Designs</h1>
								<p data-scrollax="properties: { translateY: '30%', opacity: 1.6 }">A small river named Duden flows by their place and supplies it with the necessary regelialia. It is a paradisematic country, in which roasted parts of sentences fly into your mouth.</p>
								<p><a href="#" className="btn btn-white btn-outline-white px-4 py-3 mt-3">View our works</a></p>
							</div>
						</div>
					</div>
				</div>

				<div className="slider-item js-fullheight" style={{ backgroundImage: `url(${linkImg('bg_2.jpg')})` }}>
					<div className="overlay"></div>
					<div className="container">
						<div className="row no-gutters slider-text js-fullheight align-items-center justify-content-end" data-scrollax-parent="true">
							<div className="col-md-7 text ftco-animate" data-scrollax=" properties: { translateY: '70%' }">
								<h1 className="mb-4" data-scrollax="properties: { translateY: '30%', opacity: 1.6 }">Virtually Build Your House</h1>
								<p data-scrollax="properties: { translateY: '30%', opacity: 1.6 }">A small river named Duden flows by their place and supplies it with the necessary regelialia. It is a paradisematic country, in which roasted parts of sentences fly into your mouth.</p>
								<p><a href="#" className="btn btn-white btn-outline-white px-4 py-3 mt-3">View our works</a></p>
							</div>
						</div>
					</div>
				</div>
			</section>

			<section className="ftco-services bg-light">
				<div className="container">
					<div className="row">
						<div className="col-md-4 d-flex align-self-stretch ftco-animate">
							<div className="media block-6 services d-block">
								<div className="icon d-flex justify-content-center align-items-center">
									<span className="flaticon-idea"></span>
								</div>
								<div className="media-body p-2 mt-3">
									<h3 className="heading">Perfectly Design</h3>
									<p>Even the all-powerful Pointing has no control about the blind texts it is an almost unorthographic.</p>
								</div>
							</div>
						</div>
						<div className="col-md-4 d-flex align-self-stretch ftco-animate">
							<div className="media block-6 services d-block">
								<div className="icon d-flex justify-content-center align-items-center">
									<span className="flaticon-compass-symbol"></span>
								</div>
								<div className="media-body p-2 mt-3">
									<h3 className="heading">Carefully Planned</h3>
									<p>Even the all-powerful Pointing has no control about the blind texts it is an almost unorthographic.</p>
								</div>
							</div>
						</div>
						<div className="col-md-4 d-flex align-self-stretch ftco-animate">
							<div className="media block-6 services d-block">
								<div className="icon d-flex justify-content-center align-items-center">
									<span className="flaticon-layers"></span>
								</div>
								<div className="media-body p-2 mt-3">
									<h3 className="heading">Smartly Execute</h3>
									<p>Even the all-powerful Pointing has no control about the blind texts it is an almost unorthographic.</p>
								</div>
							</div>
						</div>
					</div>
				</div>
			</section>

			<section className="ftco-section ftc-no-pb">
				<div className="container">
					<div className="row no-gutters">
						<div className="col-md-5 p-md-5 img img-2" style={{ backgroundImage: `url(${linkImg('about.jpg')})` }}>
						</div>
						<div className="col-md-7 wrap-about pb-md-5 ftco-animate">
							<div className="heading-section mb-5 pl-md-5 heading-section-with-line">
								<div className="pl-md-5 ml-md-5">
									<span className="subheading">About</span>
									<h2 className="mb-4">We are the best interior &amp; Architect Consultant in Italy</h2>
								</div>
							</div>
							<div className="pl-md-5 ml-md-5 mb-5">
								<p>Hệ thống chọn cho mình phong cách hiện đại châu Âu vì chúng tôi mê mẩn với cái đẹp, sự thanh cao, sự tinh tế đến từng đường nét. Chúng tôi lại yêu hơn với tính thực tế của phong cách này, nó không phải là một cái gì hào nhoáng, bóng bẩy, không phải sự tô son trát phấn cho công trình mà nó chính xác đi vào công năng sử dụng và sự tối ưu hóa tuyệt vời. Nó chú ý đến từng chi tiết thẩm mỹ nhỏ nhất, gần như không có bất kỳ sự thừa thãi nào. Chính vì thế nó mang lại cho bạn sự hài lòng và niềm tin thích tuyệt vời trong quá trình sử dụng với một chi phí đầu tư hợp lý.</p>
								<br />
								<p>Trong quá trình hoạt động, chúng tôi nhận thấy sự khao khát của thị trường về những sản phẩm nội thất cao cấp có "tính thẩm mỹ tuyệt vời và chất lượng tối đa" ở trên thế giới mà Việt Nam chưa có. Càng tìm hiểu về những dòng sản phẩm này Kenli càng yêu thích, say sưa về nó. Chúng tôi bắt đầu chuyển mình sang tìm kiếm, hợp tác với những hãng nội thất hàng đầu thế giới để phân phối về Việt Nam những "tuyệt phẩm" nội thất hoàn hảo từ thiết kế bên ngoài và chất lượng bên trong.</p>
								<p><a href="#" className="btn-custom">Learn More <span className="ion-ios-arrow-forward"></span></a></p>
							</div>
						</div>
					</div>
				</div>
			</section>


			<section className="ftco-section ftco-counter img" id="section-counter" style={{ backgroundImage: `url(${linkImg('bg_3.jpg')})` }} data-stellar-background-ratio="0.5">
				<div className="container">
					<div className="row d-md-flex align-items-center justify-content-center">
						<div className="col-lg-4">
							<div className="heading-section pl-md-5 heading-section-white">
								<div className="pl-md-5 ml-md-5 ftco-animate">
									<span className="subheading">Some</span>
									<h2 className="mb-4">Interesting Facts</h2>
								</div>
							</div>
						</div>
						<div className="col-lg-8">
							<div className="row d-md-flex align-items-center">
								<div className="col-md d-flex justify-content-center counter-wrap ftco-animate">
									<div className="block-18 text-center">
										<div className="text">
											<strong className="number" data-number="1">0</strong>
											<span>Years of Experienced</span>
										</div>
									</div>
								</div>
								<div className="col-md d-flex justify-content-center counter-wrap ftco-animate">
									<div className="block-18 text-center">
										<div className="text">
											<strong className="number" data-number="21">0</strong>
											<span>Happy Clients</span>
										</div>
									</div>
								</div>
								<div className="col-md d-flex justify-content-center counter-wrap ftco-animate">
									<div className="block-18 text-center">
										<div className="text">
											<strong className="number" data-number="50">0</strong>
											<span>Finished Projects</span>
										</div>
									</div>
								</div>
								<div className="col-md d-flex justify-content-center counter-wrap ftco-animate">
									<div className="block-18 text-center">
										<div className="text">
											<strong className="number" data-number="300">0</strong>
											<span>Working Days</span>
										</div>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
			</section>

			<section className="ftco-section">
				<div className="container">
					<div className="row justify-content-center mb-5 pb-2">
						<div className="col-md-7 heading-section ftco-animate">
							<h2 className="mb-4">Our Projects</h2>
							<p>A small river named Duden flows by their place and supplies it with the necessary regelialia. It is a paradisematic country, in which roasted parts of sentences</p>
						</div>
					</div>
				</div>
				<div className="container-wrap">
					<div className="row no-gutters">
						<div className="col-md-6 col-lg-3 ftco-animate">
							<div className="project">
								<img src="/src/assets/images/work-1.jpg" className="img-fluid" alt="Colorlib Template" />
								<a href="images/work-1.jpg" className="icon image-popup d-flex justify-content-center align-items-center">
									<span className="icon-expand"></span>
								</a>
							</div>
						</div>
						<div className="col-md-6 col-lg-3 ftco-animate">
							<div className="project">
								<img src="/src/assets/images/work-2.jpg" className="img-fluid" alt="Colorlib Template" />
								<a href="images/work-2.jpg" className="icon image-popup d-flex justify-content-center align-items-center">
									<span className="icon-expand"></span>
								</a>
							</div>
						</div>
						<div className="col-md-6 col-lg-3 ftco-animate">
							<div className="project">
								<img src="/src/assets/images/work-3.jpg" className="img-fluid" alt="Colorlib Template" />
								<a href="images/work-3.jpg" className="icon image-popup d-flex justify-content-center align-items-center">
									<span className="icon-expand"></span>
								</a>
							</div>
						</div>
						<div className="col-md-6 col-lg-3 ftco-animate">
							<div className="project">
								<img src={linkImg('work-4.jpg')} className="img-fluid" alt="Colorlib Template" />
								<a href="images/work-4.jpg" className="icon image-popup d-flex justify-content-center align-items-center">
									<span className="icon-expand"></span>
								</a>
							</div>
						</div>
					</div>
				</div>
			</section>

			<section className="ftco-section testimony-section">
				<div className="container">
					<div className="row justify-content-center mb-5 pb-3">
						<div className="col-md-7 heading-section ftco-animate">
							<h2 className="mb-4">Our satisfied customer says</h2>
							<p>Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts. Separated they live in</p>
						</div>
					</div>
					<div className="row ftco-animate">
						<div className="col-md-12">
							<div className="carousel-testimony owl-carousel">
								<div className="item">
									<div className="testimony-wrap p-4 pb-5">
										<div className="user-img mb-5" style={{ backgroundImage: `url(${linkImg('person_2.jpg')})` }}>
											<span className="quote d-flex align-items-center justify-content-center">
												<i className="icon-quote-left"></i>
											</span>
										</div>
										<div className="text">
											<p className="mb-5 pl-4 line">Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts.</p>
											<p className="name">Garreth Smith</p>
											<span className="position">Marketing Manager</span>
										</div>
									</div>
								</div>
								<div className="item">
									<div className="testimony-wrap p-4 pb-5">
										<div className="user-img mb-5" style={{ backgroundImage: `url(${linkImg('person_2.jpg')})` }}>
											<span className="quote d-flex align-items-center justify-content-center">
												<i className="icon-quote-left"></i>
											</span>
										</div>
										<div className="text">
											<p className="mb-5 pl-4 line">Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts.</p>
											<p className="name">Garreth Smith</p>
											<span className="position">Interface Designer</span>
										</div>
									</div>
								</div>
								<div className="item">
									<div className="testimony-wrap p-4 pb-5">
										<div className="user-img mb-5" style={{ backgroundImage: `url(${linkImg('person_3.jpg')})` }}>
											<span className="quote d-flex align-items-center justify-content-center">
												<i className="icon-quote-left"></i>
											</span>
										</div>
										<div className="text">
											<p className="mb-5 pl-4 line">Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts.</p>
											<p className="name">Garreth Smith</p>
											<span className="position">UI Designer</span>
										</div>
									</div>
								</div>
								<div className="item">
									<div className="testimony-wrap p-4 pb-5">
										<div className="user-img mb-5" style={{ backgroundImage: `url(${linkImg('person_1.jpg')})` }}>
											<span className="quote d-flex align-items-center justify-content-center">
												<i className="icon-quote-left"></i>
											</span>
										</div>
										<div className="text">
											<p className="mb-5 pl-4 line">Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts.</p>
											<p className="name">Garreth Smith</p>
											<span className="position">Web Developer</span>
										</div>
									</div>
								</div>
								<div className="item">
									<div className="testimony-wrap p-4 pb-5">
										<div className="user-img mb-5" style={{ backgroundImage: `url(${linkImg('person_3.jpg')})` }}>
											<span className="quote d-flex align-items-center justify-content-center">
												<i className="icon-quote-left"></i>
											</span>
										</div>
										<div className="text">
											<p className="mb-5 pl-4 line">Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts.</p>
											<p className="name">Garreth Smith</p>
											<span className="position">System Analyst</span>
										</div>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
			</section>

			<section className="ftco-section">
				<div className="container">
					<div className="row justify-content-center mb-5 pb-3">
						<div className="col-md-7 heading-section ftco-animate">
							<h2 className="mb-4">Our Interior</h2>
							<p></p>
						</div>
					</div>
					<div className="row">
						{interiors?.map((item) => (
							<div key={item.interiorId} className="col-md-4 ftco-animate fadeInUp ftco-animated">
								<a href={`/interior/${item.interiorId}`} className="block-20" style={{
									backgroundImage: `url(data:image/jpeg;base64,${item.image})`,
									backgroundSize: 'cover',
									backgroundPosition: 'center',
									backgroundRepeat: 'no-repeat',
									height: '300px',
									width: '70%',
									borderRadius: '5%',
								}}></a>
								<div className="blog-entry">
									<div className="text d-flex py-4">
										<div className="meta">
											<div><a href="#">{item.price}</a></div>
										</div>
										<div className="desc pl-3">
											<h3 className="heading"><a href="#">{item.interiorName}</a></h3>
										</div>
									</div>
								</div>
							</div>
						))}
					</div>
				</div>
			</section>

			<section className="ftco-section ftc-no-pb">
				<div className="container">
					<div className="row no-gutters">
						<div className="col-md-5 p-md-5 img img-2 order-md-last" style={{ backgroundImage: `url(${linkImg('img.jpg')})` }}>
						</div>
						<div className="col-md-7 wrap-about pb-md-5 ftco-animate">
							<div className="heading-section mb-md-5 pl-md-5 heading-section-with-line">
								<div className="pr-md-5 mr-md-5">
									<span className="subheading">Perfect</span>
									<h2 className="mb-4">We Make Perfection</h2>
								</div>
							</div>
							<div className="pr-md-5 mr-md-5">
								<p>On her way she met a copy. The copy warned the Little Blind Text, that where it came from it would have been rewritten a thousand times and everything that was left from its origin would be the word "and" and the Little Blind Text should turn around and return to its own, safe country. But nothing the copy said could convince her and so it didn’t take long until a few insidious Copy Writers ambushed her, made her drunk with Longe and Parole and dragged her into their agency, where they abused her for their.</p>
								<p>When she reached the first hills of the Italic Mountains, she had a last view back on the skyline of her hometown Bookmarksgrove, the headline of Alphabet Village and the subline of her own road, the Line Lane. Pityful a rethoric question ran over her cheek, then she continued her way.</p>
								<p><a href="#" className="btn-custom">Learn More <span className="ion-ios-arrow-forward"></span></a></p>
							</div>
						</div>
					</div>
				</div>
			</section>
		</>
	)
}

export default Home;